﻿using System;
using System.Diagnostics;

namespace MinecraftServerSharp.NBT
{
    public readonly partial struct NbtElement
    {
        private readonly NbtDocument _parent;
        private readonly int _index;

        public NbtElement this[int index]
        {
            get
            {
                CheckValidInstance();
                return _parent.GetContainerElement(_index, index);
            }
        }

        public NbtType TagType => _parent?.GetTagType(_index) ?? NbtType.Null;

        internal NbtElement(NbtDocument parent, int index)
        {
            // parent is usually not null, but the Current property
            // on the enumerators (when initialized as default) can
            // get here with a null.
            Debug.Assert(index >= 0);

            _parent = parent;
            _index = index;
        }

        /// <summary>
        /// Get the number of values contained within the current container element.
        /// </summary>
        /// <returns>The number of values contained within the current array value.</returns>
        /// <exception cref="InvalidOperationException">This value is not a container.</exception>
        /// <exception cref="ObjectDisposedException">The parent <see cref="NbtDocument"/> has been disposed.</exception>
        public int GetContainerLength()
        {
            CheckValidInstance();

            return _parent.GetContainerLength(_index);
        }

        public ContainerEnumerator EnumerateContainer()
        {
            CheckValidInstance();

            if (!TagType.IsContainer())
                throw new InvalidOperationException("The tag is not a container.");

            return new ContainerEnumerator(this);
        }

        public void WriteTo(NbtWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            CheckValidInstance();

            _parent.WriteTagTo(_index, writer);
        }

        /// <summary>
        ///   Get a <see cref="NbtElement"/> which can be safely stored beyond the 
        ///   lifetime of the original <see cref="NbtDocument"/>.
        /// </summary>
        /// <returns>
        ///   A <see cref="NbtElement"/> which can be safely stored beyond the 
        ///   lifetime of the original <see cref="NbtDocument"/>.
        /// </returns>
        /// <remarks>
        ///   <para>
        ///     If this NbtElement is itself the output of a previous call to Clone, or
        ///     a value contained within another NbtElement which was the output of a previous
        ///     call to Clone, this method results in no additional memory allocation.
        ///   </para>
        /// </remarks>
        public NbtElement Clone()
        {
            CheckValidInstance();

            if (!_parent.IsDisposable)
                return this;

            return _parent.CloneTag(_index);
        }

        /// <summary>
        ///   Compares <paramref name="text" /> to the string value of this element.
        /// </summary>
        /// <param name="text">The text to compare against.</param>
        /// <returns>
        ///   <see langword="true" /> if the string value of this element matches <paramref name="text"/>,
        ///   <see langword="false" /> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///   This value's <see cref="ValueKind"/> is not <see cref="JsonValueKind.String"/>.
        /// </exception>
        /// <remarks>
        ///   This method is functionally equal to doing an ordinal comparison of <paramref name="text" /> and
        ///   the result of calling <see cref="GetString" />, but avoids creating the string instance.
        /// </remarks>
        public bool StringEquals(string? text)
        {
            return StringEquals(text.AsSpan());
        }

        /// <summary>
        ///   Compares the data represented by <paramref name="data" /> to
        ///   the value of this element, either a byte array or UTF-8 text.
        /// </summary>
        /// <param name="data">The data to compare against, either a byte array or UTF-8 text.</param>
        /// <returns>
        ///   <see langword="true" /> if the value of this element is sequence equal to
        ///   <paramref name="data" />, <see langword="false" /> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///   This value is not a <see cref="NbtType.String"/> or <see cref="NbtType.ByteArray"/>.
        /// </exception>
        /// <remarks>
        ///   This method is functionally equal to doing a comparison of <paramref name="data" />
        ///   with the result of <see cref="GetString" /> or <see cref="GetByteArray"/>, 
        ///   but avoids creating the string or array instance.
        /// </remarks>
        public bool StringOrArrayEquals(ReadOnlySpan<byte> data)
        {
            CheckValidInstance();

            return _parent.StringOrByteArrayEquals(_index, data);
        }

        /// <summary>
        ///   Compares <paramref name="text" /> to the string value of this element.
        /// </summary>
        /// <param name="text">The text to compare against.</param>
        /// <returns>
        ///   <see langword="true" /> if the string value of this element matches <paramref name="text"/>,
        ///   <see langword="false" /> otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///   This value's <see cref="TagType"/> is not <see cref="NbtType.String"/>.
        /// </exception>
        /// <remarks>
        ///   This method is functionally equal to doing an ordinal comparison of <paramref name="text" /> and
        ///   the result of calling <see cref="GetString" />, but avoids creating the string instance.
        /// </remarks>
        public bool StringEquals(ReadOnlySpan<char> text)
        {
            CheckValidInstance();

            return _parent.StringEquals(_index, text);
        }

        private void CheckValidInstance()
        {
            if (_parent == null)
                throw new InvalidOperationException();
        }
    }
}
