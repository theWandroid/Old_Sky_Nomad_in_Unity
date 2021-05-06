using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//una interface no hereda y no es una clase
public interface IManager 
{

    string State { get; set; }

    void Initialize();


}
