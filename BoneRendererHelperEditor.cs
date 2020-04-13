using UnityEditor;
using System;

namespace UnityEngine.Animations.Rigging
{
    [CustomEditor(typeof(BoneRendererHelper))]
    public class BoneRendererHelperEditor : Editor
    {
        enum Mode
        {
               Dumb,
               Depth,
               DepthAdv,
               Regex
        }

        BoneRendererHelper helper;
        Mode current_mode = Mode.Dumb;

        int start_offset = 0;
        int number_of_items = 0;

        int depth_min = 0;
        int depth_max = 0;

        string regex_filter = ".*";

        private void OnEnable()
        {
            helper = (BoneRendererHelper)target;
        }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            /*
             * Filter features i want:
             * Start Offset
             * Number of items
             * Max depth
             * Min depth
             * Regex Filter
             * 
             * Modes:
             *  Dumb (offset, num)
             *  Depth (Min, max)
             *  Depth (Min, max, offset, num)
             *  Regex (C# Regex) (Todo)
             */

            EditorGUILayout.Space(2);
            current_mode = (Mode)EditorGUILayout.EnumPopup("Select mode", current_mode);

            switch (current_mode)
            {
                case Mode.Dumb:
                    start_offset = EditorGUILayout.IntField("Start offset", start_offset);
                    number_of_items = EditorGUILayout.IntField("Number of items (0 = all)", number_of_items);
                    EditorGUILayout.HelpBox(new GUIContent("Not implemented!"));
                    break;
                case Mode.Depth:
                    depth_min = EditorGUILayout.IntField("Minimum depth", depth_min);
                    depth_max = EditorGUILayout.IntField("Max depth (0 = all)", depth_max);
                    EditorGUILayout.HelpBox(new GUIContent("Not implemented!"));
                    break;
                case Mode.DepthAdv:
                    start_offset = EditorGUILayout.IntField("Start offset", start_offset);
                    number_of_items = EditorGUILayout.IntField("Number of items (0 = all)", number_of_items);
                    depth_min = EditorGUILayout.IntField("Minimum depth", depth_min);
                    depth_max = EditorGUILayout.IntField("Max depth (0 = all)", depth_max);
                    EditorGUILayout.HelpBox(new GUIContent("Not implemented!"));
                    break;
                case Mode.Regex:
                    regex_filter = EditorGUILayout.TextField("Regex filter", regex_filter);
                    EditorGUILayout.HelpBox(new GUIContent("Not implemented!"));
                    break;
            }


            if (GUILayout.Button("Build Bone Renderer"))
            {
                EditorUtility.SetDirty(this);
                EditorUtility.SetDirty(helper.boneRenderer);
                switch (current_mode)
                {
                    case Mode.Dumb:
                        runBuilderDumb();
                        break;
                    case Mode.Depth:
                        runBuilderDepth();
                        break;
                    case Mode.DepthAdv:
                        runBuilderDepthAdv();
                        break;
                    case Mode.Regex:
                        runBuilderRegex();
                        break;
                }
            }
        }

        private void runBuilderDumb()
        {
            helper.BuilderDumb(start_offset, number_of_items);
        }

        private void runBuilderDepth()
        {
            helper.BuilderDepth(depth_min, depth_max);
        }
        private void runBuilderDepthAdv()
        {
            helper.BuilderDepth(depth_min, depth_max, start_offset, number_of_items);
        }


        private void runBuilderRegex()
        {

        }
    }
}
