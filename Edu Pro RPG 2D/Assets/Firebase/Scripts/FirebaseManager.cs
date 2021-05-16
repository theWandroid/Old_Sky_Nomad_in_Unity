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

    //Verificar y corregir las dependencias de Firebase y una vez finalizado recibir el resultado y almacenarlo en una variable
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(checkDependancyTask => {
        var dependencyStatus = checkDependancyTask.Result;
        if (dependencyStatus == DependencyStatus.Available) //Si el resultado estaba disponible, inicializa Firebase
        {
            InitializeFirebase();
        }
        else //En caso de no estar disponible deoura el error
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }
    });
    }

    private void InitializeFirebase() //Al iniciar Firebase establecer la referencia de Autenticación en la instancia predeterminada
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
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
            //si no hay excepciones significa que ha iniciado sesión correctamente
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1f);
                GameManager.instance.ChangeScene(1); //cambiar a la escena del lobby
            }
            
            else
            {
                //TODO: Enviar Email Verificación
                //Temporalmente mandarlos al lobby
                GameManager.instance.ChangeScene(1);
            }
        }
        
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if(_username == "")
        {
            registerOutputText.text = "Introduce Un Nombre De Usuario";
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

                    //TODO: Enviar Email Verificación
                }
            }
        
        }
    }
}
