// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageWithCommandRow.cs $
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
    /// TElitePageWithCommandRow
    /// </summary>
    public class TElitePageWithCommandRow : TEliteMessage
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

        /// <summary>
        /// Page response message data as array
        /// </summary>
        private byte[] m_Data = null;

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
        /// Initializes a new instance of the PageWithCommandRow class.
        /// </summary>
        public TElitePageWithCommandRow()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.PageWithCommandRow;
        }

        /// <summary>
        /// Initializes a new instance of the PageWithCommandRow class.
        /// </summary>
        public TElitePageWithCommandRow(byte[] messageFrame)
            : this()
        {
            this.m_Data = messageFrame;
        }

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TElitePageWithCommandRow()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts PageWithCommandRow message to a byte array
        /// </summary>
        /// <returns>byte array</returns>
        public override byte[] ToArray()
        {
            return this.m_Data;
        }

        /// <summary>
        /// Converts PageWithCommandRow message to a Ep1 file
        /// </summary>
        /// <returns>EP1 file</returns>
        public EP1File ToEP1File()
        {
            EP1File ep1File = new EP1File();
            ep1File.Initialize();

            if (m_Data.Length > 0)
            {
                int pos = 0;
                while ((pos < m_Data.Length) && (m_Data[pos] != 0xFF))
                {
                    // copy row data
                    byte[] rowData = new byte[40];
                    for (int i = 0; i < 40; i++)
                    {
                        rowData[i] = m_Data[pos + i + 1];
                    }

                    // copy row zero
                    if (m_Data[pos] == 0)
                    {
                        ep1File.SetByteHeaderRow(rowData);
                    }
                    // copy line 1 to 23
                    if ((m_Data[pos] > 0) && (m_Data[pos] <= 23))
                    {
                        ep1File.SetByteLine(rowData, m_Data[pos], 0);
                    }
                    // copy line 24
                    if (m_Data[pos] == 24)
                    {
                        ep1File.SetByteCommandRow(rowData);
                    }

                    pos += 41;
                }
            }

            return ep1File;
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

        #region Web-Methods
        //**************************************************
        // Web-Methods
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageWithCommandRow.cs $
 * 
 * 3     30.06.09 11:15 san
 * 
 * 2     23.06.09 14:45 san
 * 
 * 1     22.06.09 13:35 san
 * -------------------------------------------------------------------
 */