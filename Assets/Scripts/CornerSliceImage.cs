using UnityEngine;
using UnityEngine.UI;

// Custom UI Graphic logic for that one specific type of textures
// - left-top corner texture with stretch-able 1-pixel wide center, top and bottom
// Doesn't support Sprite Border (it's easy, but won't make any difference in this case)
// DOES support atlases
// One mesh -> no tears on GPU side whatsoever, perfect texture interpolation every time
[ExecuteInEditMode]
public class CornerSliceImage : Graphic
{
   public Sprite image;

   public override Texture mainTexture => image == null ? s_WhiteTexture : image.texture;

   protected override void OnPopulateMesh(VertexHelper vh)
   {
      if (image != null)
      {
         var imageSize = image.rect;
         var rect = rectTransform.rect;
         var pivot = rectTransform.pivot;
         
         // Won't handle reduced size for now, not worth the trouble
         var minSize = new Vector2(imageSize.width * 2f, imageSize.height * 2f);
         rect.width = Mathf.Max(minSize.x, rect.width);
         rect.height = Mathf.Max(minSize.y, rect.height);
      
         Vector2 leftCorner = Vector2.zero;
         Vector2 rightCorner = Vector2.zero;

         leftCorner.x = 0f;
         leftCorner.y = 0f;
         rightCorner.x = 1f;
         rightCorner.y = 1f;

         leftCorner.x -= pivot.x;
         leftCorner.y -= pivot.y;
         rightCorner.x -= pivot.x;
         rightCorner.y -= pivot.y;

         leftCorner.x *= rect.width;
         leftCorner.y *= rect.height;
         rightCorner.x *= rect.width;
         rightCorner.y *= rect.height;

         vh.Clear();
         
         var vert = UIVertex.simpleVert;
         
         var zeroX = image.uv[2].x;
         var zeroY = image.uv[2].y;
         var oneX = image.uv[1].x;
         var oneY = image.uv[1].y;

         // First row
         var currentYLine = leftCorner.y;
         vert.position = new Vector2(leftCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(leftCorner.x + imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x - imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, oneY);
         vh.AddVert(vert);
         
         // Second row
         currentYLine = leftCorner.y + imageSize.height;
         vert.position = new Vector2(leftCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(leftCorner.x + imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x - imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, zeroY);
         vh.AddVert(vert);

         // Third row
         currentYLine = rightCorner.y - imageSize.height;
         vert.position = new Vector2(leftCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(leftCorner.x + imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x - imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, zeroY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, zeroY);
         vh.AddVert(vert);

         // Fourth row
         currentYLine = rightCorner.y;
         vert.position = new Vector2(leftCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(leftCorner.x + imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x - imageSize.width, currentYLine);
         vert.uv0 = new Vector2(oneX, oneY);
         vh.AddVert(vert);
         
         vert.position = new Vector2(rightCorner.x, currentYLine);
         vert.uv0 = new Vector2(zeroX, oneY);
         vh.AddVert(vert);
         
         vh.AddTriangle(0, 5, 1);
         vh.AddTriangle(0, 4, 5);
         
         vh.AddTriangle(1, 6, 2);
         vh.AddTriangle(1, 5, 6);

         vh.AddTriangle(2, 7, 3);
         vh.AddTriangle(2, 6, 7);
         
         vh.AddTriangle(4, 8, 9);
         vh.AddTriangle(4, 9, 5);
         
         vh.AddTriangle(5, 10, 6);
         vh.AddTriangle(5, 9, 10);
         
         vh.AddTriangle(6, 11, 7);
         vh.AddTriangle(6, 10, 11);
         
         vh.AddTriangle(8, 13, 9);
         vh.AddTriangle(8, 12, 13);
         
         vh.AddTriangle(9, 14, 10);
         vh.AddTriangle(9, 13, 14);
         
         vh.AddTriangle(10, 15, 11);
         vh.AddTriangle(10, 14, 15);
      }
      else
      {
         base.OnPopulateMesh(vh);
      }
   }
}
