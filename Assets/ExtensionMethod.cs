using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace test
{
    public static class ExtensionMethod 
    {
        public static T DeepCopy<T>(this T value) where T : class,new()
        {
            T t = new T();
           
            return t;
        }
  
    }

}
