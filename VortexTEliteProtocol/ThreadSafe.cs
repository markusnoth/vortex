// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/ThreadSafe.cs $
// $Author: fun $
// $Date: 2010-11-08 17:18:34 +0100 (Mo, 08 Nov 2010) $
// $Revision: 82 $
//
// Description:
//
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// This is a helper class to call event-handlers in a thread safe manner.
    /// Use this class for event-handlers that are used in Windows Forms.
    /// </summary>
    public static class ThreadSafe
    {

        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Invoke methode to call events thread safe
        /// </summary>
        /// <param name="method">delegate methode</param>
        /// <param name="args">arguments of delegate</param>
        public static void Invoke(Delegate method, object[] args)
        {
            if (method != null)
            {
                foreach (Delegate handler in method.GetInvocationList())
                {
                    if (handler.Target is Control)
                    {
                        Control target = handler.Target as Control;

                        if (target.IsHandleCreated)
                        {
                            target.BeginInvoke(handler, args);
                        }
                    }
                    else if (handler.Target is ISynchronizeInvoke)
                    {
                        ISynchronizeInvoke target = handler.Target as ISynchronizeInvoke;
                        target.BeginInvoke(handler, args);
                    }
                    else
                    {
                        handler.DynamicInvoke(args);
                    }
                }
            }
        }
        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/ThreadSafe.cs $
 * 
 * 3     22.06.09 13:35 san
 * 
 * 2     22.06.09 10:54 san
 * 
 * 1     22.06.09 8:34 san
 * -------------------------------------------------------------------
 */