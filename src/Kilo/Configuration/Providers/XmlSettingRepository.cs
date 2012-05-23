using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Kilo.Configuration.Providers
{
	public class XmlSettingRepository : ISettingRepository, ILoadable, IPersistable
	{
		string _path;
		XElement _root;

		public bool CanLoad
		{
			get
			{
				return File.Exists(_path);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlSettingRepository"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public XmlSettingRepository(string path)
		{
			this._path = path;
		}

		/// <summary>
		/// Writes the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		public void WriteSetting(string name, object value, string group = null, string options = null)
		{
			XElement root = _root;

			if (string.IsNullOrWhiteSpace(group) == false)
			{
				var groupRoot = _root.Element(group);

				if (groupRoot == null)
				{
					groupRoot = new XElement(group);
					_root.Add(groupRoot);
				}

				root = groupRoot;
			}

			if (root.Element(name) != null)
				root.Element(name).Value = value.ToString();
			else
				root.Add(new XElement(name, value));
		}

		/// <summary>
		/// Reads the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		public object ReadSetting(string name, string group = null, string options = null)
		{
			XElement root = _root;

			if (string.IsNullOrWhiteSpace(group) == false)
			{
				var groupRoot = _root.Element(group);

				if (groupRoot == null)
				{
					return null;
				}

				root = groupRoot;
			}

			if (root != null)
				return root.Element(name).Value;
			else
				return null;
		}

		/// <summary>
		/// Determines whether the specified name has setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns>
		///   <c>true</c> if the specified name has setting; otherwise, <c>false</c>.
		/// </returns>
		public bool HasSetting(string name, string group = null, string options = null)
		{
			XElement root = _root;

			if (string.IsNullOrWhiteSpace(group) == false)
			{
				var groupRoot = _root.Element(group);

				if (groupRoot == null)
					return false;

				root = groupRoot;
			}

			return (root != null) && (root.Element(name) != null);
		}

		/// <summary>
		/// Loads this instance.
		/// </summary>
		public void Load()
		{
			if (this.CanLoad)
				_root = XElement.Load(_path);
			else
			{
				_root = new XElement("settings");
			}
		}

		/// <summary>
		/// Persists this instance.
		/// </summary>
		public void Persist()
		{
			_root.Save(_path);
		}
	}
}
