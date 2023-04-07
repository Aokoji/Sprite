using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TableManager : CSingel<TableManager>
{
    TableConfig config;
    public bool loadsuccess { get { return config.loadsuccess; } }

    public void init()
    {
        config = new TableConfig();
        config.init();
    }

}
