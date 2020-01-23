// Copyright (c) SimControl e.U. - Wilhelm Medetz. See LICENSE.txt in the project root for more information.

using System;
using System.Globalization;
using SimControl.Log;

namespace SimControl.Templates.CSharp.WcfServiceLibrary
{
    [Log]
    public class Service1: IService1
    {
        public string GetData(int value) => value.ToString(CultureInfo.InvariantCulture);

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null) throw new ArgumentNullException(nameof(composite));

            return composite;
        }
    }
}
