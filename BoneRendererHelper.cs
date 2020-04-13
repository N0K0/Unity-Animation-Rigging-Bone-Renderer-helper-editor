using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
    [ExecuteInEditMode]
    [AddComponentMenu("Animation Rigging/Setup/Bone Renderer Helper")]
    public class BoneRendererHelper : MonoBehaviour
    {
        [SerializeField]
        public BoneRenderer boneRenderer;

        [SerializeField]
        GameObject rootObject;

        private void Reset()
        {
            boneRenderer = gameObject.GetComponent<BoneRenderer>();
            if(rootObject == null)
            {
                rootObject = gameObject.transform.Find("Root").gameObject;
            }
        }

        public void BuilderDumb(int start_offset, int number_of_items)
        {
            if (start_offset < 0)
            {
                Debug.LogError("start_offset can not be under 0");
                return;
            }

            if (number_of_items < 0)
            {
                Debug.LogError("number_of_items can not be under 0");
                return;
            }

            if (rootObject == null)
            {
                Debug.LogError("rootObject is null! Please set the root for the rig");
            }

            Transform[] objects = GetComponentsInChildren<Transform>();
            
            if(number_of_items == 0)
            {
                number_of_items = objects.Length - start_offset;
            }
            Transform[] slice = objects.Skip(start_offset).Take(number_of_items).ToArray();

            boneRenderer.transforms = slice;
        }

        public void BuilderDepth(int depth_min, int depth_max)
        {
            BuilderDepth(depth_min, depth_max, 0, 0);
        }
        internal void BuilderDepth(int depth_min, int depth_max, int start_offset, int number_of_items)
        {

            List<Tuple<int, Transform>> search_queue = new List<Tuple<int, Transform>>();
            List<Tuple<int, Transform>> return_queue = new List<Tuple<int, Transform>>();
            if (depth_min < 0)
            {
                Debug.LogError("depth_min can not be under 0");
                return;
            }

            if (depth_max < 0)
            {
                Debug.LogError("depth_max can not be under 0");
                return;
            };

            List<Tuple<int, Transform>> children = new List<Tuple<int, Transform>>();

            search_queue.Add(Tuple.Create(0, rootObject.transform));

            if (depth_max == 0)
            {
                depth_max = int.MaxValue;
            }

            while (search_queue.Any())
            {
                var child = search_queue[0];
                search_queue.RemoveAt(0);

                int depth = child.Item1;
                Transform transform = child.Item2;
               
                for(int i = 0; i < transform.childCount; i++) {
                    var child_trans = transform.GetChild(i);
                    // We stop if we are too deep, nothing good can come from searching further
                    if ( depth <= depth_max)
                    {
                        search_queue.Insert(0, Tuple.Create(depth + 1, child_trans));
                        return_queue.Insert(0, Tuple.Create(depth + 1, child_trans));
                    }
                }
            }

            List<Transform> result_accum = new List<Transform>();
            // Filter too shallow result when we are done. No way ever the rig three is too big to be reasonable
            for(int i = 0; i < return_queue.Count; i++ )
            {
                var child = return_queue[i];
                int depth = child.Item1;
                Transform transform = child.Item2;

                if( depth >= depth_min)
                {
                    result_accum.Add(transform);
                }
            }

            if(number_of_items == 0 )
            {
                number_of_items = result_accum.Count - start_offset;
            }

            var results = result_accum.ToArray();
            var slice = results.Skip(start_offset).Take(number_of_items);

            boneRenderer.transforms = slice.ToArray();
        }

        public void BuilderRegex()
        {

        }
    }
}
