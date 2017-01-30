// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteCommandLine.cs $
// $Author: har $
// $Date: 2011-06-14 11:17:47 +0200 (Di, 14 Jun 2011) $
// $Revision: 656 $
//
// Description:
//
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteCommandLine
    /// </summary>
    public class TEliteCommandLine : TEliteMessage
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************

        #endregion


        #region Constants
        //**************************************************
        // Constants
        //**************************************************

        #endregion


        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Public fields
        //**************************************************
        // Public fields
        //**************************************************

        #endregion

        #region Protected fields
        //**************************************************
        // Protected fields
        //**************************************************

        #endregion

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// Command Line data
        /// </summary>
        private string m_CommandLine = string.Empty;

        #endregion

        #endregion


        #region Properties
        //**************************************************
        // Properties
        //**************************************************

        #region Public properties
        //**************************************************
        // Public properties
        //**************************************************

        /// <summary>
        /// Get Command Line text
        /// </summary>
        public string CommandLine
        {
            get { return m_CommandLine; }
        }

        #endregion

        #region Protected properties
        //**************************************************
        // Protected properties
        //**************************************************

        #endregion

        #region Private properties
        //**************************************************
        // Private properties
        //**************************************************

        #endregion

        #endregion


        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Constructor
        //**************************************************
        // Constructor
        //**************************************************

        /// <summary>
        /// Initializes a new instance of the TEliteCommandLine class.
        /// </summary>
        public TEliteCommandLine()
        {
            this.m_MessageOrigin = MessageOriginEnum.Client;
            this.m_Cmd = MessageTypeCode.CommandLine;
        }

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteCommandLine()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Login Command to Vortex
        /// </summary>
        /// <param name="username">username</param>
        public void Login(string username)
        {
            this.m_CommandLine = String.Format("log {0}", username);
        }

        /// <summary>
        /// Send password to Vortex
        /// </summary>
        /// <param name="password">password as plaintext string</param>
        public void Password(string password)
        {
            this.m_CommandLine = password;
        }

        /// <summary>
        /// Logout Command from Vortex
        /// </summary>
        public void Logout()
        {
            this.m_CommandLine = "bye";
        }

        /// <summary>
        /// Set command line message
        /// </summary>
        /// <param name="commandLine">command line string</param>
        public void SetCommandLine(string commandLine)
        {
            this.m_CommandLine = commandLine;
        }

        /// <summary>
        /// Upload an EP1 file to Vortex
        /// </summary>
        /// <param name="commandLine">Command line macro</param>
        public void Upload(string commandLine)
        {
            this.m_CommandLine = commandLine;
        }

        /// <summary>
        /// Upload an EP1 file to Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        public void Upload(int magazin, int set, int page)
        {
            this.m_CommandLine = String.Format("COIN {0:000} {1:00} {2:00}", magazin, set, page);
        }

        /// <summary>
        /// Download an EP1 file from Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        public void Download(int magazin, int set, int page)
        {
            this.m_CommandLine = String.Format("{0:000} {1:00}.{2:00}", magazin, set, page);
        }

        /// <summary>
        /// Attributes of Page from Vortex
        /// </summary>
        /// <param name="magazin">Magazin number</param>
        /// <param name="set">Set number</param>
        /// <param name="page">Page number</param>
        public void Attributes(int magazin, int set, int page)
        {
            if ((magazin >= 0) && (set >= 0) && (page >= 0))
            {
                this.m_CommandLine = String.Format("attr {0:000} {1:00}.{2:00}", magazin, set, page);
            }
            else
            {
                if ((magazin >= 0) && (set >= 0))
                {
                    this.m_CommandLine = String.Format("attr {0:000} {1:00}", magazin, set);
                }
                else
                {
                    if (magazin >= 0)
                    {
                        this.m_CommandLine = String.Format("attr {0:000}", magazin);
                    }
                }
            }
        }

        /// <summary>
        /// Converts a CommandLine message to a byte array
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            byte[] messageFrame = new byte[80];
            char[] command = m_CommandLine.ToCharArray();

            for (int i = 0; i < messageFrame.Length; i++)
            {
                if (i < command.Length)
                {
                    // copy command line text
                    messageFrame[i] = (byte)command[i];
                }
                else
                {
                    // set space char for fill to the end
                    messageFrame[i] = 0x20;
                }
            }

            return messageFrame;
        }

        #endregion

        #region Protected Methods
        //**************************************************
        // Protected Methods
        //**************************************************

        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteCommandLine.cs $
 * 
 * 12    16.07.09 16:27 san
 * 
 * 11    16.07.09 11:40 san
 * 
 * 10    15.07.09 14:15 san
 * Add new function: Get Page-Attribute EP1 from Vortex.
 * 
 * 9     1.07.09 9:45 san
 * 
 * 8     23.06.09 16:06 san
 * 
 * 7     23.06.09 14:45 san
 * 
 * 6     22.06.09 16:24 san
 * 
 * 5     22.06.09 10:54 san
 * 
 * 4     19.06.09 15:01 san
 * 
 * 3     18.06.09 14:57 san
 * 
 * 2     17.06.09 16:29 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */