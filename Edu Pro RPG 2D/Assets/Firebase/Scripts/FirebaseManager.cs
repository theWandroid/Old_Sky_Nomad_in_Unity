using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
    [Space(5f)]

    [Header("Login Reference")]
    [SerializeField]
    private TMP_InputField loginEmail;
    [SerializeField]
    private TMP_InputField loginPassword;
    [SerializeField]
    private TMP_Text loginOutputText;
    [Space(5f)]

    [Header("Register Reference")]
    [SerializeField]
    private TMP_InputField registerUsername;
    [SerializeField]
    private TMP_InputField registerEmail;
    [SerializeField]
    private TMP_InputField registerPassword;
    [SerializeField]
    private TMP_InputField registerConfirmPassword;
    [SerializeField]
    private TMP_Text registerOutputText;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        //Iniciar coroutina
        StartCoroutine(CheckAndFixDependencies());
    }
    //Verificar y corregir las dependencias de Firebase y una vez finalizado recibir el resultado y almacenarlo en una variable
    private IEnumerator CheckAndFixDependencies()
        {
            var checkAndFixDependanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();
          //Esperar hasta que la tarea esté completada
        yield return new WaitUntil(predicate: () => checkAndFixDependanciesTask.IsCompleted);
        //obtener resultado
        var dependancyResult = checkAndFixDependanciesTask.Result;
        //inicializar firebase
        if(dependancyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
       
        else
        {
            //En caso de haber algún error dar un Debug.Log
            Debug.LogError($"No se han podido resolver todas las dependencias de Firebase: {dependancyResult}");
        }
        }

    private void InitializeFirebase() //Al iniciar Firebase establecer la referencia de Autenticación en la instancia predeterminada
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        //verificar si hay un usuario
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);
            AutoLogin();
        }
        else
        {
            //Si no hay usuario enviarlo a la pagina de login
            AuthUIManager.instance.LoginScreen();
        }
    }
    
    private void AutoLogin()
    {
      
        if(user != null)
        {
            //Ver si está verificado
            if (user.IsEmailVerified) 
            { 
            //Si está el Email Verificado Cambiar a la escena
            GameManager.instance.ChangeScene(1);
            }
            //Si no enviar Email de verificación{
            StartCoroutine(SendVerificationEmail());
        }
        else
        {
            //Si no hay usuario enviarlo a la pagina de login
            AuthUIManager.instance.LoginScreen();
        }
    }

 

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        //Verificar que auth.CurrentUser no es igual a nuestra referencia de usuario. Si lo es eso solo significa que el estado de autenticación cambió pero no el del usuario
        if (auth.CurrentUser != user)
        {
            //Comprobar si estamos iniciando sesión
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

                //Desconectarlo si no hemos iniciado sesión y el usuario anterior no era igual
                if(!signedIn && user != null)
            {
                Debug.Log("Desconectado");
            }
                //Usuario actual ha iniciado sesión
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"Conectado En: {user.DisplayName}");
            }
        }
    }

    public void ClearOutputs() //mostrar cualquier error o información que el usuario necesita saber
    {
        loginOutputText.text = "";
        registerOutputText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        //llamar al metodo inicio de sesión de Firebase con credencial asincrónica pasando esa credencial
        var loginTask = auth.SignInWithCredentialAsync(credential);
        //esperar a que se complete la tarea (var loginTask)
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        //verificar si hay una excepción y en caso afirmativo obtener la excepción base y cambiarla a AuthError
        if (loginTask.Exception !=null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "Error Desconocido, Porfavor Intentalo Otra Vez";
            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Porfavor Introduce Tu Email";
                    break;
                case AuthError.MissingPassword:
                    output = "Porfavor Introduce Tu Contraseña";
                    break;
                case AuthError.InvalidEmail:
                    output = "Email Invalido";
                    break;
                case AuthError.WrongPassword:
                    output = "Contraseña Invalida";
                    break;
                case AuthError.UserNotFound:
                    output = "Esa Cuenta No Existe";
                    break;
            }
            loginOutputText.text = output;
        }
        else
        {
            //Si no hay excepciones significa que ha iniciado sesión correctamente
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1f);
                //Cambiar a la escena del lobby
                GameManager.instance.ChangeScene(1); 
            }
            else
            {
                //Enviar Email Verificación
                StartCoroutine(SendVerificationEmail());
            }
        }
        
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if(_username == "")
        {
            registerOutputText.text = "Porfavor Introduce Un Nombre De Usuario";
        }
        else if(_password != _confirmPassword)
        {
            registerOutputText.text = "Las Contraseñas No Coinciden";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);
            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "Error Desconocido, Porfavor Intentalo Otra Vez";
                switch (error)
                {
                    case AuthError.InvalidEmail:
                        output = "Email Invalido";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        output = "Email Ya En Uso";
                        break;
                    case AuthError.WeakPassword:
                        output = "Contraseña Débil";
                        break;
                    case AuthError.MissingEmail:
                        output = "Porfavor Introduce Tu Email";
                        break;
                    case AuthError.MissingPassword:
                        output = "Porfavor Introduce Tu Contraseña";
                        break;
                }
                registerOutputText.text = output;
            }
            else
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,
                    //TODO: Dar Foto Perfil Por Defecto
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);
                //comprobar si hay errores
                if(defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "Error Desconocido, Porfavor Intentalo Otra Vez";
                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "Actualización De Usuario Cancelada";
                            break;
                        case AuthError.SessionExpired:
                            output = "Sesión Expirada";
                            break;
                    }
                    registerOutputText.text = output;
                }
                //Si no hay errores
                else
                {
                    Debug.Log($"Usuario Firebase Creado Con Éxito: {user.DisplayName} ({user.UserId})");

                    //Enviar Email Verificación
                    StartCoroutine(SendVerificationEmail());
                }
            }
        
        }
    }
    //Enviar correo de verificacion
    private IEnumerator SendVerificationEmail()
    {
        //comprobar si hay un usuario
        if(user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);
            if(emailTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "Error Desconocido, Intentalo de Nuevo";
                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "Tarea de Verificación ha sido cancelada";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "Email Invalido";
                        break;
                    case AuthError.TooManyRequests:
                        output = "Demasiadas Peticiones";
                        break;
                }
                //Llamar a la espera de verificación porque no se ha enviado un email
                AuthUIManager.instance.AwaitVerification(false, user.Email, output);
                //si no hay errores se envió correctamente.
                AuthUIManager.instance.AwaitVerification(true, user.Email, null);
                Debug.Log("Email Enviado Con Éxito");
            }
        }
    }
}
