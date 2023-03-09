using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace radj307.ChainConverter
{
    /// <summary>
    /// Converts given values with a list of given <see cref="IValueConverter"/> converters in the order that they appear. Supports both <see cref="Convert(object, Type, object, CultureInfo)"/> &amp; <see cref="ConvertBack(object, Type, object, CultureInfo)"/>.<br/>
    /// Implements <see cref="IValueConverter"/> itself, so it can be used in place of a traditional value converter.
    /// </summary>
    public sealed class ChainConverter : IValueConverter, IList<IValueConverter>
    {
        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ChainConverter"/> instance.
        /// </summary>
        public ChainConverter() => Items ??= new();
        #endregion Constructor

        #region Properties
        /// <summary>
        /// Any number of converters that implement <see cref="IValueConverter"/>.<br/>
        /// When <see cref="Convert(object, Type, object, CultureInfo)"/> is called, its parameters are passed to the Convert method of each converter in the list, in order.
        /// </summary>
        public List<IValueConverter> Items
        {
            get; init;
        }
        #endregion Properties

        #region IValueConverter
        /// <summary>
        /// Calls the <see cref="IValueConverter.Convert(object, Type, object, CultureInfo)"/> method of each converter in <see cref="Items"/> in sequential order and passes the resulting value along to the next converter in the list, along with all other parameters.
        /// </summary>
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var item in this)
            {
                if (item is IValueConverter converter)
                {
                    value = converter.Convert(value, targetType, parameter, culture);
                }
            }
            return value;
        }
        /// <summary>
        /// Calls the <see cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)"/> method of each converter in <see cref="Items"/> in reverse-sequential order and passes the resulting value along to the next converter in the list, along with all other parameters.
        /// </summary>
        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var item in this.AsEnumerable().Reverse())
            {
                if (item is IValueConverter converter)
                {
                    value = converter.ConvertBack(value, targetType, parameter, culture);
                }
            }
            return value;
        }
        #endregion IValueConverter

        #region IList
        /// <inheritdoc/>
        public IValueConverter this[int index] { get => ((IList<IValueConverter>)this.Items)[index]; set => ((IList<IValueConverter>)this.Items)[index] = value; }
        /// <inheritdoc/>
        public int Count => ((ICollection<IValueConverter>)this.Items).Count;
        /// <inheritdoc/>
        public bool IsReadOnly => ((ICollection<IValueConverter>)this.Items).IsReadOnly;
        /// <inheritdoc/>
        public void Add(IValueConverter item) => ((ICollection<IValueConverter>)this.Items).Add(item);
        /// <inheritdoc/>
        public void Clear() => ((ICollection<IValueConverter>)this.Items).Clear();
        /// <inheritdoc/>
        public bool Contains(IValueConverter item) => ((ICollection<IValueConverter>)this.Items).Contains(item);
        /// <inheritdoc/>
        public void CopyTo(IValueConverter[] array, int arrayIndex) => ((ICollection<IValueConverter>)this.Items).CopyTo(array, arrayIndex);
        /// <inheritdoc/>
        public IEnumerator<IValueConverter> GetEnumerator() => ((IEnumerable<IValueConverter>)this.Items).GetEnumerator();
        /// <inheritdoc/>
        public int IndexOf(IValueConverter item) => ((IList<IValueConverter>)this.Items).IndexOf(item);
        /// <inheritdoc/>
        public void Insert(int index, IValueConverter item) => ((IList<IValueConverter>)this.Items).Insert(index, item);
        /// <inheritdoc/>
        public bool Remove(IValueConverter item) => ((ICollection<IValueConverter>)this.Items).Remove(item);
        /// <inheritdoc/>
        public void RemoveAt(int index) => ((IList<IValueConverter>)this.Items).RemoveAt(index);
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Items).GetEnumerator();
        #endregion IList
    }
}
