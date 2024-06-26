﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace AssetCatalog.Converters
{
	public class EnumDescriptionTypeConverter : EnumConverter
	{
		public EnumDescriptionTypeConverter(Type type)
			: base(type)
		{
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
				return base.ConvertTo(context, culture, value, destinationType);

			if (value == null)
				return string.Empty;

			var fi = value.GetType().GetField(value.ToString());

			if (fi == null)
				return string.Empty;

			var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description))
				return attributes[0].Description;

			return value.ToString();
		}
	}
}