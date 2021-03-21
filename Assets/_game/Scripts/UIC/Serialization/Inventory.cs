using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
	public class PlayerPrefsProperty<T> : System.IDisposable where T : System.IConvertible, System.IEquatable<T>
	{
		private T m_Value;
		private string m_Key;
		public event System.Action<T> OnChange;
		private bool m_Load;
		
		public PlayerPrefsProperty(string key)
		{
			m_Key = key;
		}
		
		public void Load()
		{
			m_Value = (T)System.Convert.ChangeType(PlayerPrefs.GetString(m_Key, default(T)?.ToString() ?? ""), typeof(T));
			m_Load = true;
		}

		public T Value
		{
			get
			{
				if (!m_Load)
				{
					Load();
				}
				return m_Value;
			}
			set
			{
				m_Value = value;
				PlayerPrefs.SetString(m_Key, m_Value.ToString());
				OnChange?.Invoke(m_Value);
			}
		}

		public static implicit operator T(PlayerPrefsProperty<T> d) => d.Value;

		public void Dispose()
		{
			m_Value = default(T);
			PlayerPrefs.SetString(m_Key, m_Value?.ToString() ?? "");
		}
	}

	public interface IDataResolver
	{
		bool Resolve(string data, out string resolvedData);
	}

	[System.Serializable]
	public class PlayerPrefsData<T> where T : new()
	{
		[SerializeField] private T m_Value;

		private string m_Key;
		public event System.Action<T> OnChange;
		private IDataResolver m_DataResolver;

		public PlayerPrefsData(string key, IDataResolver dataResolver)
		{
			m_Key = key;
			m_DataResolver = dataResolver;

			var s = PlayerPrefs.GetString(m_Key, "");
			if (m_DataResolver != null &&
				m_DataResolver.Resolve(s, out string rS))
			{
				Debug.Log($"Resolved from: {s} to: {rS}");
				s = rS;
			}
			if (string.IsNullOrEmpty(s)) m_Value = new T();
			else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
		}

		public PlayerPrefsData(string key, T defaultValue = default)
		{
			m_Key = key;
			var s = PlayerPrefs.GetString(m_Key, "");
			if (string.IsNullOrEmpty(s)) m_Value = defaultValue;
			else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
		}

		public T Value
		{
			get { return m_Value; }
			set
			{
				m_Value = value;
				Save();
			}
		}

		public static implicit operator T(PlayerPrefsData<T> d) => d.Value;

		public void Save()
		{
			PlayerPrefs.SetString(m_Key, JsonUtility.ToJson(this));
			OnChange?.Invoke(m_Value);
		}

		public void Dispose()
		{
			m_Value = default(T);
			PlayerPrefs.SetString(m_Key, "");
		}
	}

	public class Inventory : StrictSingleton<Inventory>
	{
		public PlayerPrefsData<int> SaveVersion;
		
		public PlayerPrefsData<AudioStatus> audioStatus;

		public PlayerPrefsData<PlayerProfile> playerProfile;
		
		[Button]
		void SaveAll()
		{
			audioStatus.Save();

			playerProfile.Save();
		}

		protected override void Setup()
		{
			Debug.LogError("~~~~~~~~ Inventory.Awake is called");
			
			DontDestroyOnLoad(gameObject);

			playerProfile = new PlayerPrefsData<PlayerProfile>("PlayerProfile", PlayerProfile.CreateDefault());

			audioStatus = new PlayerPrefsData<AudioStatus>("audioStatus", AudioStatus.CreateDefault());

			Migrate();
			
			Debug.Log("Inventory: Migration successful!");
		}

		private void Migrate()
		{
			var gameSaveVersion = Migration.saveVersion;
			SaveVersion = new PlayerPrefsData<int>("SAVE_VERSION", gameSaveVersion);
			SaveVersion.Save();

			if (SaveVersion > gameSaveVersion) throw new UnityException($"Cannot migrate {SaveVersion.Value}->{gameSaveVersion}");

			Debug.Log($"STARTING VERSION MIGRATION: {SaveVersion.Value}->{gameSaveVersion}");
			while (SaveVersion.Value < gameSaveVersion)
			{
				Migration.Step(SaveVersion);
				SaveVersion.Value++;
				SaveVersion.Save();
			}
			Debug.Log($"SAVE VERSION IS NOW CURRENT: {SaveVersion.Value}");
		}

		public void SkipTutorial()
		{
			
		}
	}
}

