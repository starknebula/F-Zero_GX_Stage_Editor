// Created by Raphael "Stark" Tetreault 10/07/2016
// Copyright © 2016 Raphael Tetreault
// Last updated 10/07/2016

namespace UnityEngine
{
    /// <summary>
    /// Extensions for Unity's Transform class.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Clears this Transform via Destroy(all children)
        /// </summary>
        /// <param name="transform">The current transform.</param>
        public static void ClearTransform(this Transform transform)
        {
            foreach (Transform child in transform.GetChildren())
                Object.Destroy(child.gameObject);
        }
        /// <summary>
        /// Clears this Transform by DestroyImmediately(all children)
        /// </summary>
        /// <param name="transform">The current transform.</param>
        public static void ClearTransformImmediate(this Transform transform)
        {
            foreach (Transform child in transform.GetChildren())
                Object.DestroyImmediate(child.gameObject);
        }

        /// <summary>
        /// Reset Transform Position to (0, 0, 0), Rotation to (0, 0, 0), and LocalScale to (1, 1, 1)
        /// </summary>
        /// <param name="transform">The current transform.</param>
        public static void ResetTransform(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }
        /// <summary>
        /// Reset local transform Position to (0, 0, 0), Rotation to (0, 0, 0, 0), and Scale to (1, 1, 1)
        /// </summary>
        /// <param name="transform">The current transform.</param>
        public static void ResetLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Returns an array of all tranforms of all children of this transform.
        /// </summary>
        /// <param name="parent">The current transform.</param>
        public static Transform[] GetChildren(this Transform parent)
        {
            Transform[] children = new Transform[parent.childCount];

            for (int i = 0; i < parent.childCount; i++)
                children[i] = parent.GetChild(i);

            return children;
        }


        //
        public static Vector3[] GetPositions(this Transform[] transforms)
        {
            Vector3[] positions = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                positions[i] = transforms[i].position;

            return positions;
        }
        public static Vector3[] GetLocalPositions(this Transform[] transforms)
        {
            Vector3[] localPositions = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                localPositions[i] = transforms[i].localPosition;

            return localPositions;
        }

        public static Quaternion[] GetRotations(this Transform[] transforms)
        {
            Quaternion[] rotations = new Quaternion[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                rotations[i] = transforms[i].rotation;

            return rotations;
        }
        public static Quaternion[] GetLocalRotations(this Transform[] transforms)
        {
            Quaternion[] localRotations = new Quaternion[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                localRotations[i] = transforms[i].localRotation;

            return localRotations;
        }
        public static Vector3[] GetEulerRotations(this Transform[] transforms)
        {
            Vector3[] rotations = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                rotations[i] = transforms[i].rotation.eulerAngles;

            return rotations;
        }
        public static Vector3[] GetEulerLocalRotations(this Transform[] transforms)
        {
            Vector3[] localRotations = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                localRotations[i] = transforms[i].localRotation.eulerAngles;

            return localRotations;
        }

        public static Vector3[] GetLocalScales(this Transform[] transforms)
        {
            Vector3[] localScales = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                localScales[i] = transforms[i].localScale;

            return localScales;
        }
        public static Vector3[] GetLossyScales(this Transform[] transforms)
        {
            Vector3[] lossyScales = new Vector3[transforms.Length];

            for (int i = 0; i < transforms.Length; i++)
                lossyScales[i] = transforms[i].lossyScale;

            return lossyScales;
        }
    }
}