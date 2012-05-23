using System;
using System.Linq;
using System.Reflection;
using Kilo.Configuration.Providers;

namespace Kilo.Configuration
{
    public class SettingsManager
    {
        ISettingRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingManager"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SettingsManager(ISettingRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Read settings for the specified type
        /// </summary>
        /// <typeparam name="T">The settings type</typeparam>
        public T Read<T>() where T : class
        {
            return Read(typeof(T)) as T;
        }

        /// <summary>
        /// Read settings for the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An instance of the specified type, populated with settings</returns>
        public object Read(Type type)
        {
            if (_repository is ILoadable)
                ((ILoadable)_repository).Load();

            object instance = CreateInstance(type);

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attributes = prop.GetCustomAttributes(typeof(SettingAttribute), true).Cast<SettingAttribute>();

                string outputName = prop.Name;
                string options = null;
                string group = null;

                if (attributes.Any())
                {
                    SettingAttribute settingAttrib = attributes.First();

                    if (settingAttrib.Ignore)
                        continue;

                    if (!string.IsNullOrWhiteSpace(settingAttrib.Name))
                        outputName = settingAttrib.Name;

                    options = settingAttrib.Options;
                    group = settingAttrib.Group;

                }

                if (_repository.HasSetting(outputName, group, options))
                {
                    object facadeValue = _repository.ReadSetting(outputName, group, options);
                    object converted = Convert.ChangeType(facadeValue, prop.PropertyType);

                    prop.SetValue(instance, converted, null);
                }
            }

            return instance;
        }

        /// <summary>
        /// Writes the specified settings using the underlying repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings">The settings.</param>
        public void Write<T>(T settings) where T : class
        {
            if (_repository is ILoadable)
                ((ILoadable)_repository).Load();

            Type type = typeof(T);

            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attributes = prop.GetCustomAttributes(typeof(SettingAttribute), true).Cast<SettingAttribute>();

                string outputName = prop.Name;
                string options = null;
                string group = null;
                object propValue = prop.GetValue(settings, null);

                if (attributes.Any())
                {
                    SettingAttribute settingAttrib = attributes.First();

                    if (settingAttrib.Ignore)
                        continue;

                    if (!string.IsNullOrWhiteSpace(settingAttrib.Name))
                        outputName = settingAttrib.Name;

                    options = settingAttrib.Options;
                    group = settingAttrib.Group;
                }

                _repository.WriteSetting(outputName, propValue, group, options);
            }

            if (_repository is IPersistable)
                ((IPersistable)_repository).Persist();
        }

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The instance of the specified type.</returns>
        protected virtual object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
