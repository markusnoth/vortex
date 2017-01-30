// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/VortexAttributes.cs $
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
    /// VortexAttributes
    /// </summary>
    public class VortexAttributes
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************
        /// <summary>
        /// AttributeTypeEnum
        /// </summary>
        public enum AttributeTypeEnum
        {
            /// <summary>
            /// None
            /// </summary>
            None, 
            /// <summary>
            /// Magazine
            /// </summary>
            Magazine, 
            /// <summary>
            /// Set
            /// </summary>
            Set, 
            /// <summary>
            /// Page
            /// </summary>
            Page
        };

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

        private EP1File m_Ep1File = null;
        private AttributeTypeEnum m_Type = AttributeTypeEnum.None;
        private AttributesValues m_Values = null;

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
        /// Gets or sets the Type
        /// </summary>
        public AttributeTypeEnum Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        /// <summary>
        /// Gets or sets atrreibute values
        /// </summary>
        public AttributesValues Values
        {
            get { return m_Values; }
            set { m_Values = value; }
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
        /// Initializes a new instance of the VortexAttributes class.
        /// </summary>
        public VortexAttributes(EP1File ep1File)
        {
            m_Ep1File = ep1File;
            this.Parse(ep1File);
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~VortexAttributes()
        {

        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

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

        /// <summary>
        /// Parse EP1-File data and assign properties
        /// </summary>
        /// <param name="ep1File"></param>
        private void Parse(EP1File ep1File)
        {
            string pageLine = string.Empty;
            m_Values = null;
            m_Type = AttributeTypeEnum.None;

            pageLine = ep1File.GetLine(1, false);
            if (pageLine.Contains("Attributes for Magazine"))
            {
                m_Type = AttributeTypeEnum.Magazine;
                // Parse attributes for a magazine
                m_Values = new MagazineAttributes(ep1File);
            }
            if (pageLine.Contains("Attributes for Set"))
            {
                m_Type = AttributeTypeEnum.Set;
                // Parse attributes for a set
                m_Values = new SetAttributes(ep1File);
            }
            if (pageLine.Contains("Attributes for Page"))
            {
                m_Type = AttributeTypeEnum.Page;
                // Parse attributes for a page
                m_Values = new PageAttributes(ep1File);
            }
        }

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
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/VortexAttributes.cs $
 * 
 * 2     16.07.09 16:27 san
 * 
 * 1     16.07.09 11:42 san
 * -------------------------------------------------------------------
 */