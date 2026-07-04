using static WingLoader.Scripting.Checks;

namespace WingLoader.Scripting
{
    /// <summary>
    /// Represents a single evaluation constraint on a given piece of the games memory
    /// i.e. it describes the memory object to be tested (e.g. mem_Message1)
    /// the format of the checking test (e.g. string contains) 
    /// and defines an action to take when the the condition is met.
    /// </summary>
    internal class WingLoaderRule
    {
        /// <summary> The referenced memory address monitored by this rule context. </summary>
        public string memAddress;
        /// <summary> The expected value string sequence to be checked against the memory. </summary>
        public string memValue;
        /// <summary> The operator used to validate the memory content. </summary>
        public CheckTypes memCheckType;
        /// <summary> A list of actions or operational behaviors triggered when this rule validates as true. </summary>
        public List<Activity> activities = new List<Activity>();

        /// <summary>
        /// Initializes a customized instance of the <see cref="WingLoaderRule"/> class with a single starter activity.
        /// </summary>
        /// <param name="address">The target memory string .</param>
        /// <param name="checkType">The matching test type. </param>
        /// <param name="value">The value to test the memory against (via checkType).</param>
        /// <param name="activities">The response activity when the checkType is successful. </param>
        internal WingLoaderRule(string Address, CheckTypes CheckType, string Value, Activity Activities)
        {
            memAddress = Address;
            memValue = Value;
            memCheckType = CheckType;
            activities.Add(Activities);
        }

        /// <summary>
        /// Generates a standardized, empty fallback baseline <see cref="WingLoaderRule"/> instance to represent a blank state.
        /// </summary>
        /// <returns>A dummy rule structure configured with an address of -1 and a check type of nothing.</returns>
        internal static WingLoaderRule NullRule()
        {
            return new WingLoaderRule("-1", CheckTypes.nothing, "", Activity.NullActivity());
        }

        /// <summary>
        /// Compares two rule instances for value equality based on address, check logic, and value criteria.
        /// </summary>
        public static bool operator ==(WingLoaderRule x, WingLoaderRule y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return ((x.memAddress == y.memAddress) &&
                (x.memValue == y.memValue) &&
                (x.memCheckType == y.memCheckType));
        }

        /// <summary>
        /// Compares two rule instances for structural value inequality.
        /// </summary>
        public static bool operator !=(WingLoaderRule x, WingLoaderRule y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Validates value equality with an external, strongly typed <see cref="WingLoaderRule"/> target instance.
        /// </summary>
        private bool Equals(WingLoaderRule? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (this == other);
        }

        /// <summary>
        /// Validates value equality against a generic object reference.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return Equals(obj as WingLoaderRule);
        }

        /// <summary>
        /// Computes a unique hash code tracking identifier corresponding to the value properties of this instance layout.
        /// </summary>
        /// <returns>An integer representation hash code calculation index.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(memAddress, memValue, memCheckType);
        }
    }
}
