using UnityEngine;

namespace Utility {
    /// <summary>
    /// A class for polar coordinates (2D).
    /// </summary>
    public class Polar {
        public float Magnitude;
        public float Phase;

        public Vector2 Cartesian {
            get { return new Vector2(Magnitude * Mathf.Cos(Phase), Magnitude * Mathf.Sin(Phase)); }
            set {
                Magnitude = value.magnitude;
                Phase = Mathf.Atan2(value.y, value.x);
            }
        }

        public Vector3 Cartesian3D {
            get { return new Vector3(Magnitude * Mathf.Cos(Phase), 0.0f, Magnitude * Mathf.Sin(Phase)); }
        }

        public Polar(float magnitude, float phase) {
            Magnitude = magnitude;
            Phase = phase;
        }

        public Polar(Vector2 cartesian) {
            Cartesian = cartesian;
        }

        public static Polar zero {
            get { return new Polar(0.0f, 0.0f); }
        }
    }

    public static class VectorPolarExtensions {
        public static Polar ToPolar(this Vector2 vector) {
            Polar polar = Polar.zero;
            polar.Cartesian = vector;
            return polar;
        }
    }
}