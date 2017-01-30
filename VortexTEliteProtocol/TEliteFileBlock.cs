// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteFileBlock.cs $
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
using System.IO;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteFileBlock
    /// </summary>
    public class TEliteFileBlock : TEliteMessage
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
        /// Command line response message data as array
        /// </summary>
        private byte[] m_Data = null;

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
        /// Gets the encapsulated file records
        /// </summary>
        public byte[] Data
        {
            get { return m_Data; }
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
        /// Initializes a new instance of the TEliteCommandResponse class.
        /// </summary>
        public TEliteFileBlock()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.FileBlock;
        }

        /// <summary>
        /// Initializes a new instance of the TEliteCommandResponse class.
        /// </summary>
        public TEliteFileBlock(byte[] messageFrame)
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
        ~TEliteFileBlock()
        {

        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts CommandResponse message to a byte array
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            return this.m_Data;
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
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteFileBlock.cs $
 * 
 * 2     20.07.09 13:34 Not
 * command line responses in file format can be handled now
 * 
 * 1     16.07.09 14:41 Not
 * file block functionality (incomplete)
 * 
 * 5     19.06.09 15:01 san
 * 
 * 4     18.06.09 16:23 san
 * 
 * 3     18.06.09 14:57 san
 * 
 * 2     17.06.09 16:29 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */