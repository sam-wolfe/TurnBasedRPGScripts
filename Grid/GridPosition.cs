using System;

namespace Grid {

    public struct GridPosition : IEquatable<GridPosition> {

        public int x;
        public int z;

        public GridPosition(int x, int z) {
            this.x = x;
            this.z = z;
        }

        public override string ToString() {
            return $"x: {x}, z: {z}";
        }

        public static bool operator ==(GridPosition a, GridPosition b) {
            return a.x == b.x && a.z == b.z;
        }
    
        public static bool operator !=(GridPosition a, GridPosition b) {
            return a.x != b.x || a.z != b.z;
        }
    
    
        public bool Equals(GridPosition other) {
            return x == other.x && z == other.z;
        }

        public override bool Equals(object obj) {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(x, z);
        }

        public static GridPosition operator +(GridPosition a, GridPosition b) {
            return new GridPosition(a.x + b.x, a.z + b.z);
        }
        
        public static GridPosition operator -(GridPosition a, GridPosition b) {
            return new GridPosition(a.x - b.x, a.z - b.z);
        }

    }

}
