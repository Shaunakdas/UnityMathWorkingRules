using System.Collections.Generic;
using UnityEngine;


namespace TexDrawLib
{
    // Box containing horizontal stack of child boxes.
    public class HorizontalBox : Box
    {
        public float childBoxesTotalwidth = 0f;
        public bool ExtensionMode = false;

        public static HorizontalBox Get(Box box, float width, TexAlignment alignment)
        {
            var Box = Get();
            if (box.width >= width) {
                Box.Add(box);
                return Box;
            }
            var extrawidth = Mathf.Max(width - box.width, 0);
            if (alignment == TexAlignment.Center) {
                var strutBox = StrutBox.Get(extrawidth / 2f, 0, 0, 0);
                Box.Add(strutBox);
                Box.Add(box);
                Box.Add(strutBox);
            } else if (alignment == TexAlignment.Left) {
                Box.Add(box);
                Box.Add(StrutBox.Get(extrawidth, 0, 0, 0));
            } else if (alignment == TexAlignment.Right) {
                Box.Add(StrutBox.Get(extrawidth, 0, 0, 0));
                Box.Add(box);
            }
            return Box;
        }

        public static HorizontalBox Get(Box box)
        {
            var Box = ObjPool<HorizontalBox>.Get();
            if (Box.children == null) 
                Box.children = new List<Box>(8);
            Box.Add(box);
            return Box;
        }

        public static HorizontalBox Get()
        {
            var Box = ObjPool<HorizontalBox>.Get();
            if (Box.children == null) 
                Box.children = new List<Box>(8);
            return Box;
        }

        public static HorizontalBox Get(Box[] box)
        {
            var Box = ObjPool<HorizontalBox>.Get();
            if (Box.children == null) 
                Box.children = new List<Box>(box.Length);
            for (int i = 0; i < box.Length; i++) {
                Box.Add(box[i]);
            }
            return Box;
        }

        //Specific for DrawingParams
        public static HorizontalBox Get(List<Box> box)
        {
            var Box = ObjPool<HorizontalBox>.Get();
            if (Box.children == null) 
                Box.children = new List<Box>(box.Count);
            for (int i = 0; i < box.Count; i++) {
                Box.Add(box[i]);
            }
            ListPool<Box>.ReleaseNoFlush(box);
            return Box;
        }

        //Specific for DrawingParams
        public void AddRange(List<Box> box)
        {
            for (int i = 0; i < box.Count; i++) {
                Add(box[i]);
            }
            ListPool<Box>.ReleaseNoFlush(box);
        }

        public void AddRange(Box box)
        {
            for (int i = 0; i < box.children.Count; i++) {
                Add(box.children[i]);
            }
        }
        
        public void AddRange(Box box, int position)
        {
            for (int i = 0; i < box.children.Count; i++) {
                Add(position++, box.children[i]);
            }
        }

        public override void Add(Box box)
        {            
            childBoxesTotalwidth += box.width;
            
            if (children.Count == 0) {
                height = float.NegativeInfinity;
                depth = float.NegativeInfinity;
            }
            
            height = Mathf.Max(height, box.height - box.shift);
            depth = Mathf.Max(depth, box.depth + box.shift);
            width = Mathf.Max(width, childBoxesTotalwidth);
            
            base.Add(box);
        }

        public override void Add(int position, Box box)
        {
            childBoxesTotalwidth += box.width;
            
            if (children.Count == 0) {
                height = float.NegativeInfinity;
                depth = float.NegativeInfinity;
            }
            
            height = Mathf.Max(height, box.height - box.shift);
            depth = Mathf.Max(depth, box.depth + box.shift);
            width = Mathf.Max(width, childBoxesTotalwidth);
            
            base.Add(position, box);
          }
          
        public void Recalculate()
        {
            childBoxesTotalwidth = 0;
            width = 0;
            height = children.Count == 0 ? 0 : float.NegativeInfinity;
            depth = children.Count == 0 ? 0 : float.NegativeInfinity;
            for (int i = 0; i < children.Count; i++)
            {
                var box = children[i];
                childBoxesTotalwidth += box.width;
                height = Mathf.Max(height, box.height - box.shift);
                depth = Mathf.Max(depth, box.depth + box.shift);
                width = Mathf.Max(width, childBoxesTotalwidth);
            
            }
        }

        public override void Draw(DrawingContext drawingContext, float scale, float x, float y)
        {
            base.Draw(drawingContext, scale, x, y);

            var curX = x;
            if (ExtensionMode) {
                float offset = TEXConfiguration.main.ExtentPadding * 2;
                for (int i = 0; i < children.Count; i++) {
                    Box child = children[i];
                    var extWidth = (i == 0 || i == children.Count - 1) ? offset : offset * 2;
                    {
                        child.width += extWidth;
                        if (child is CharBox)
                            ((CharBox)child).italic += extWidth;
                        else if (child is RotatedCharBox)
                            ((RotatedCharBox)child).italic += extWidth;
                    }
                    if (i > 0)
                        curX -= offset;
                    child.Draw(drawingContext, scale, curX, y - child.shift);
                    {
                        child.width -= extWidth;
                        if (child is CharBox)
                            ((CharBox)child).italic -= extWidth;
                        else if (child is RotatedCharBox)
                            ((RotatedCharBox)child).italic -= extWidth;
                    }
                    if (i > 0)
                        curX += offset;
                    curX += child.width;
                }
            } else {
                for (int i = 0; i < children.Count; i++) {
                    Box box = children[i];
                    box.Draw(drawingContext, scale, curX, y - box.shift);
                    curX += box.width;
                }
            }
        }

        public override void Flush()
        {
            base.Flush();
            childBoxesTotalwidth = 0;
            ExtensionMode = false;
            ObjPool<HorizontalBox>.Release(this);
        }
    }
}