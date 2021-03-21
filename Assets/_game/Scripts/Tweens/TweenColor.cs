using UnityEngine;
using UnityEngine.UI;

namespace TSG.Tweens
{
    public class TweenColor : TweenBase 
{
	#region Fields

    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    [SerializeField] GameObject target = null;

    Material[] materials;
    Graphic uiGraphicElement;

    public Color StartColor 
    { 
        get
        { 
            return startColor; 
        }
        set 
        { 
            startColor = value; 
        }
    }

    public Color EndColor 
    { 
        get
        { 
            return endColor; 
        }
        set 
        { 
            endColor = value; 
        }
    }

	#endregion	


	#region Properties

    Color Color
    {
        get
        {
            //UI Graphic element
            if (uiGraphicElement != null)
            {
                return uiGraphicElement.color;
            }

            //MeshRenderer Materials Color
            if (materials != null && materials.Length > 0)
            {
                return materials[0].color;
            }

            return endColor;
        }

        set
        {
            //UI Image Color
            if (uiGraphicElement != null)
            {
                uiGraphicElement.color = value;
            }

            //MeshRenderer Materials Color
            if (materials != null)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    var m = materials[i];
                    m.color = value;
                }
            }
        }
    }

	#endregion

	
	#region Unity Lifecycle

    protected override void Awake()
    {


        base.Awake();

        if (target != null)
        {
            uiGraphicElement = target.GetComponent<Graphic>();
       
            MeshRenderer mr = target.GetComponent<MeshRenderer>();

            if (mr != null)
            {
                materials = mr.sharedMaterials;    
            }
        }
        else
        {
            uiGraphicElement = GetComponent<Graphic>();

            MeshRenderer mr = GetComponent<MeshRenderer>();

            if (mr != null)
            {
                materials = mr.sharedMaterials;    
            }
        }

    }


	#endregion


	#region Public Methods
	#endregion


	#region Private Methods

    protected override void UpdateTweenWithFactor(float factor)
    {
        Color = Color.Lerp(startColor, endColor, factor);

    }

	#endregion


	#region Event Handlers
	#endregion
}

}
