using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class IcwXMLLevels
{

    private xmlLevel[] levelField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("level")]
    public xmlLevel[] level
    {
        get
        {
            return this.levelField;
        }
        set
        {
            this.levelField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class xmlLevel
{

    private xmlLevelEnemy[] enemyField;

    private string nameField;

    private byte numberField;

    private string backgroundField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("enemy")]
    public xmlLevelEnemy[] enemy
    {
        get
        {
            return this.enemyField;
        }
        set
        {
            this.enemyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string background
    {
        get
        {
            return this.backgroundField;
        }
        set
        {
            this.backgroundField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class xmlLevelEnemy
{

    private string nameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }
}

