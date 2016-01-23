using UnityEngine;

public class UWOffset : MonoBehaviour
{
  private Material thisMaterial = null;
  public float OffsetX = 0;

	private void Start ()
  {
    thisMaterial = GetComponent<MeshRenderer>().material;
  }	
	
	private void Update ()
  {	  
    thisMaterial.mainTextureOffset = new Vector2(OffsetX, 0);
  }
}
