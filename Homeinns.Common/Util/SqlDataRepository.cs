using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Homeinns.Common.Configuration;
using Homeinns.Common.Util;
using SQLite;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 本地SQLite数据的基础操作类封装
	/// </summary>
	public static class SqlDataRepository
	{
		private static SQLiteConnection _dbConnection;

		/// <summary>
		/// 公用的Locker对象，所用针对本地数据库的操作都要调用C#的lock进行锁定
		/// </summary>
		public static object DbOpenLockerObject = new object();

		/// <summary>
		/// 判断本地数据库是否已经初始化并打开
		/// </summary>
		public static bool IsOpened
		{
			get { return _dbConnection != null; }
		}

		private static readonly List<Action> InitializeActions = new List<Action>();

		public static void AddInitializeActions(Action action)
		{
			InitializeActions.Add(action);
		}

		/// <summary>
		/// 根据用户代码初始化本地数据库
		/// </summary>
		/// <param name="userCode"></param>
		public static void Initialize(string userCode)
		{
			lock (DbOpenLockerObject)
			{
				var dbFilePath = Path.Combine(FileSystemUtil.CachesFolder, "db");
				if (!Directory.Exists(dbFilePath))
					Directory.CreateDirectory(dbFilePath);

				_dbConnection = null;
				SQLite3.Config(SQLite3.ConfigOption.Serialized);
				var dbName = Path.Combine(dbFilePath, string.Format("AppData_{0}_" + AppGlobalSetting.LocalDbVersion + ".db", userCode));
				//_dbConnection = new SQLiteConnection (dbName);
				_dbConnection = new SQLiteConnection(dbName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);

				CreateTableOrIndexActions.ForEach(action =>
				{
					action();
				});

				InitializeActions.ForEach(action =>
				{
					action();
				});
			}
		}


		private static readonly List<Action> CreateTableOrIndexActions = new List<Action>();

		/// <summary>
		/// 使用LINQ的方式在本地数据库中创建表
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void CreateTable<T>()
		{
			if (_dbConnection != null)
			{
				_dbConnection.CreateTable<T>();
			}
			CreateTableOrIndexActions.Add(() =>
			{
				_dbConnection.CreateTable<T>();
			});
		}

		/// <summary>
		/// 使用LINQ的方式在本地数据库中创建表的索引
		/// </summary>
		public static void CreateIndex<T>(Expression<Func<T, object>> property, bool unique = false)
		{
			if (_dbConnection != null)
			{
				_dbConnection.CreateIndex(property, unique);
			}

			CreateTableOrIndexActions.Add(() =>
			{
				_dbConnection.CreateTable<T>();
			});
		}

		/// <summary>
		/// 使用LINQ的方式获取可执行查询的Query
		/// </summary>
		public static TableQuery<T> Table<T>()
			where T : new()
		{
			return _dbConnection.Table<T>();
		}

		/// <summary>
		/// 在本地SQLLite数据库中插入纪录
		/// </summary>
		/// <param name="obj">待插入的对象</param>
		/// <returns></returns>
		public static int Insert(object obj)
		{
			return _dbConnection.Insert(obj);
		}

		/// <summary>
		/// 更新本地SQLite数据库中的对象纪录
		/// </summary>
		/// <param name="obj">待更新的对象</param>
		/// <returns></returns>
		public static int Update(object obj)
		{
			return _dbConnection.Update(obj);
		}

		/// <summary>
		/// 删除本地SQLite数据库中的纪录
		/// </summary>
		/// <param name="obj">待删除的对象</param>
		/// <returns></returns>
		public static int Delete(object obj)
		{
			return _dbConnection.Delete(obj);
		}

		/// <summary>
		/// Commit数据库操作
		/// </summary>
		public static void Commit()
		{
			_dbConnection.Commit();
		}
		/// <summary>
		/// 启动一个事物
		/// </summary>
		public static void BeginTransaction()
		{
			_dbConnection.BeginTransaction();
		}
		/// <summary>
		/// 回滚操作
		/// </summary>
		public static void Rollback()
		{
			_dbConnection.Rollback();
		}
	}
}

