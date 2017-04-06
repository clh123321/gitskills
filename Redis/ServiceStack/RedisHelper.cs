using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Redis.Common
{
    /// <summary>
    /// RedisHelper
    /// </summary>
    public static class RedisHelper
    {
        static string m_WriteRedisHost = string.Empty;
        static string m_ReadRedisHost = string.Empty;
        static int m_RedisDB = 0;
        static int m_CacheTime = 0;

        static PooledRedisClientManager clientManager;
        /// <summary>
        /// 
        /// </summary>
        static RedisHelper()
        {
            if (string.IsNullOrWhiteSpace(ApplictionCacheConfig.RedisReadHostAddress) || string.IsNullOrWhiteSpace(ApplictionCacheConfig.RedisWriteHostAddress))
                throw new Exception("RedisWriteHostAddress or RedisReadHostAddress missed");
            m_WriteRedisHost = ApplictionCacheConfig.RedisWriteHostAddress;
            m_ReadRedisHost = ApplictionCacheConfig.RedisReadHostAddress;
            if (ApplictionCacheConfig.RedisDb == null || !int.TryParse(ApplictionCacheConfig.RedisDb, out m_RedisDB))
                throw new ConfigurationErrorsException("RedisDb missed or formateError");
            if (ApplictionCacheConfig.RedisCacheTime == null || !int.TryParse(ApplictionCacheConfig.RedisCacheTime, out m_CacheTime))
                m_CacheTime = 30;

            clientManager = new RedisManager().CreateManager(m_WriteRedisHost, m_ReadRedisHost, m_RedisDB);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IRedisClient GetClient()
        {
            return clientManager.GetClient();
        }

        #region string操作类、List操作类

        #region 赋值
        /// <summary>
        /// 从左侧向list中添加值-入栈
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void LPush(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PushItemToList(key, value);
            }
        }
        /// <summary>
        /// 从左侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public static void LPush(string key, string value, DateTime dt)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PushItemToList(key, value);
                redis.ExpireEntryAt(key, dt);
            }
        }
        /// <summary>
        /// 从左侧向list中添加值，设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
        public static void LPush(string key, string value, TimeSpan sp)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PushItemToList(key, value);
                redis.ExpireEntryIn(key, sp);
            }
        }
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void RPush(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PrependItemToList(key, value);
            }
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public static void RPush(string key, string value, DateTime dt)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PrependItemToList(key, value);
                redis.ExpireEntryAt(key, dt);
            }
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
 
        public static void RPush(string key, string value, TimeSpan sp)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.PrependItemToList(key, value);
                redis.ExpireEntryIn(key, sp);
            }
        }
        /// <summary>
        /// 添加key/value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
  
        public static void Add(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddItemToList(key, value);
            }
        }
        /// <summary>
        /// 添加key/value ,并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dt"></param>
        public static void Add(string key, string value, DateTime dt)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddItemToList(key, value);
                redis.ExpireEntryAt(key, dt);
            }
        }
        /// <summary>
        /// 添加key/value。并添加过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="sp"></param>
        public static void Add(string key, string value, TimeSpan sp)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddItemToList(key, value);
                redis.ExpireEntryIn(key, sp);
            }
        }
        /// <summary>
        /// 为key添加多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void Add(string key, List<string> values)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddRangeToList(key, values);
            }
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="dt"></param>
        public static void Add(string key, List<string> values, DateTime dt)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddRangeToList(key, values);
                redis.ExpireEntryAt(key, dt);
            }
        }
        /// <summary>
        /// 为key添加多个值，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="sp"></param>
        public static void Add(string key, List<string> values, TimeSpan sp)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddRangeToList(key, values);
                redis.ExpireEntryIn(key, sp);
            }
        }
        #endregion

        #region 获取值
        /// <summary>
        /// 获取list中key包含的数据数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Count(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetListCount(key);
            }
        }
        /// <summary>
        /// 获取key包含的所有数据集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> Get(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllItemsFromList(key);
            }
        }
        /// <summary>
        /// 获取key中下标为star到end的值集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<string> Get(string key, int star, int end)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeFromList(key, star, end);
            }
        }
        #endregion

        #region 阻塞命令
        /// <summary>
        /// 阻塞命令：从list中keys的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string BlockingDequeueItemFromList(string key, TimeSpan? sp)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.BlockingDequeueItemFromList(key, sp);
            }
        }
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string BlockingPopItemFromList(string key, TimeSpan? sp)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.BlockingPopItemFromList(key, sp);
            }
        }
        
        /// <summary>
        /// 阻塞命令：从list中key的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string BlockingRemoveStartFromList(string key, TimeSpan? sp)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.BlockingRemoveStartFromList(key, sp);
            }
        }
        
        /// <summary>
        /// 阻塞命令：从list中一个fromkey的尾部移除一个值，添加到另外一个tokey的头部，并返回移除的值，阻塞时间为sp
        /// </summary>
        /// <param name="fromkey"></param>
        /// <param name="tokey"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static string BlockingPopAndPushItemBetweenLists(string fromkey, string tokey, TimeSpan? sp)
        {
            //using (IRedisClient redis = GetClient())
            //{
            //    return redis.BlockingPopAndPushItemBetweenLists(fromkey, tokey, sp);
            //}
            return string.Empty;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 从尾部移除数据，返回移除的数据-出栈 无堵塞
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PopItemFromList(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PopItemFromList(key);
            }
        }
        
        /// <summary>
        /// 移除list中，key/value,与参数相同的值，并返回移除的数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long RemoveItemFromList(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveItemFromList(key, value);
            }            
        }
        /// <summary>
        /// 从list的尾部移除一个数据，返回移除的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveEndFromList(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveEndFromList(key);
            }
        }
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveStartFromList(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveStartFromList(key);
            }
        }
        #endregion

        #region 其它
        /// <summary>
        /// 从一个list的尾部移除一个数据，添加到另外一个list的头部，并返回移动的值
        /// </summary>
        /// <param name="fromKey"></param>
        /// <param name="toKey"></param>
        /// <returns></returns>
        public static string PopAndPushItemBetweenLists(string fromKey, string toKey)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PopAndPushItemBetweenLists(fromKey, toKey);
            }
        }
        #endregion

        #endregion 

        #region Hash操作类
        #region 添加
        /// <summary>
        /// 向hashid集合中添加key/value
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 说明：如果Key不存在，该命令将创建新Key以参数中的Field/Value对，如果参数中的Field在该Key中已经存在，则用新值覆盖其原有值
     
        public static bool SetEntryInHash(string hashid, string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.SetEntryInHash(hashid, key, value);
            }
        }
        /// <summary>
        /// 如果hashid集合中存在key/value则不添加返回false，如果不存在在添加key/value,返回true
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 只在指定的field不存在的情况下，才设置field的值 备注：暂时保留
        public static bool SetEntryInHashIfNotExists(string hashid, string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.SetEntryInHashIfNotExists(hashid, key, value);
            }
        }
        /// <summary>
        /// 存储对象T t到hash集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void StoreAsHash<T>(T t)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.StoreAsHash<T>(t);
            }
        }
        #endregion
        #region 获取
        /// <summary>
        /// 获取对象T中ID为id的数据。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetFromHash<T>(object id)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetFromHash<T>(id);
            }
        }
        /// <summary>
        /// 获取所有hashid数据集的key/value数据集合
        /// </summary>
        /// <param name="hashid"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllEntriesFromHash(string hashid)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllEntriesFromHash(hashid);
            }
        }
        /// <summary>
        /// 获取hashid数据集中的数据总数
        /// </summary>
        /// <param name="hashid"></param>
        /// <returns></returns>
        public static long GetHashCount(string hashid)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetHashCount(hashid);
            }
        }
        /// <summary>
        /// 获取hashid数据集中所有key的集合
        /// </summary>
        /// <param name="hashid"></param>
        /// <returns></returns>
        public static List<string> GetHashKeys(string hashid)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetHashKeys(hashid);
            }
        }
        /// <summary>
        /// 获取hashid数据集中的所有value集合
        /// </summary>
        /// <param name="hashid"></param>
        /// <returns></returns>
        public static List<string> GetHashValues(string hashid)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetHashValues(hashid);
            }
        }
        /// <summary>
        /// 获取hashid数据集中，key的value数据
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueFromHash(string hashid, string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetValueFromHash(hashid, key);
            }
        }
        /// <summary>
        /// 获取hashid数据集中，多个keys的value集合
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<string> GetValuesFromHash(string hashid, string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetValuesFromHash(hashid, keys);
            }
        }
        #endregion

        /// <summary>
        /// 删除hashid数据集中的key数据
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveEntryFromHash(string hashid, string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveEntryFromHash(hashid, key);
            }
        }
        #region 其它
        /// <summary>
        /// 判断hashid数据集中是否存在key的数据
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HashContainsEntry(string hashid, string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.HashContainsEntry(hashid, key);
            }
        }
        /// <summary>
        /// 给hashid数据集key的value加countby，返回相加后的数据
        /// </summary>
        /// <param name="hashid"></param>
        /// <param name="key"></param>
        /// <param name="countBy"></param>
        /// <returns></returns>
        public static int IncrementValueInHash(string hashid, string key, int countBy)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.IncrementValueInHash(hashid, key, countBy);
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// key集合中添加value值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddItemToSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddItemToSet(key, value);
            }
        }

        /// <summary>
        /// key集合中添加list集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        public static void AddRangeToSet(string key, List<string> list)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.AddRangeToSet(key, list);
            }
        }

        /// <summary>
        /// 随机获取key集合中的一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRandomItemFromSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRandomItemFromSet(key);
            }
        }

        /// <summary>
        /// 获取key集合值的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetCount(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetSetCount(key);
            }
        }
        /// <summary>
        /// 获取所有key集合的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HashSet<string> GetAllItemsFromSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllItemsFromSet(key);
            }
        }

        #region 删除
        /// <summary>
        /// 随机删除key集合中的一个值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PopItemFromSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PopItemFromSet(key);
            }
        }
        /// <summary>
        /// 删除key集合中的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void RemoveItemFromSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.RemoveItemFromSet(key, value);
            }
        }
        #endregion

        #region 其它
        /// <summary>
        /// 从fromkey集合中移除值为value的值，并把value添加到tokey集合中
        /// </summary>
        /// <param name="fromkey"></param>
        /// <param name="tokey"></param>
        /// <param name="value"></param>
        public static void MoveBetweenSets(string fromkey, string tokey, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.MoveBetweenSets(fromkey, tokey, value);
            }
        }
        /// <summary>
        /// 返回keys多个集合中的并集，返还hashset
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static HashSet<string> GetUnionFromSets(string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetUnionFromSets(keys);
            }
        }
        /// <summary>
        /// keys多个集合中的并集，放入newkey集合中
        /// </summary>
        /// <param name="newkey"></param>
        /// <param name="keys"></param>
        public static void StoreUnionFromSets(string newkey, string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.StoreUnionFromSets(newkey, keys);
            }
        }
        /// <summary>
        /// 把fromkey集合中的数据与keys集合中的数据对比，fromkey集合中不存在keys集合中，则把这些不存在的数据放入newkey集合中
        /// </summary>
        /// <param name="newkey"></param>
        /// <param name="fromkey"></param>
        /// <param name="keys"></param>
        public static void StoreDifferencesFromSet(string newkey, string fromkey, string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.StoreDifferencesFromSet(newkey, fromkey, keys);
            }
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加key/value，默认分数是从1.多*10的9次方以此递增的,自带自增效果
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddItemToSortedSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.AddItemToSortedSet(key, value);
            }
        }
        /// <summary>
        /// 添加key/value,并设置value的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool AddItemToSortedSet(string key, string value, double score)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.AddItemToSortedSet(key, value, score);
            }
        }
        /// <summary>
        /// 为key添加values集合，values集合中每个value的分数设置为score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool AddRangeToSortedSet(string key, List<string> values, double score)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.AddRangeToSortedSet(key, values, score);
            }
        }
        /// <summary>
        /// 为key添加values集合，values集合中每个value的分数设置为score
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool AddRangeToSortedSet(string key, List<string> values, long score)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.AddRangeToSortedSet(key, values, score);
            }
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取key的所有集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromSortedSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllItemsFromSortedSet(key);
            }
        }
        /// <summary>
        /// 获取key的所有集合，倒叙输出
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetAllItemsFromSortedSetDesc(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllItemsFromSortedSetDesc(key);
            }
        }
        /// <summary>
        /// 获取可以的说有集合，带分数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetAllWithScoresFromSortedSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetAllWithScoresFromSortedSet(key);
            }
        }
        /// <summary>
        /// 获取key为value的下标值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetItemIndexInSortedSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetItemIndexInSortedSet(key, value);
            }
        }
        /// <summary>
        /// 倒叙排列获取key为value的下标值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetItemIndexInSortedSetDesc(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetItemIndexInSortedSetDesc(key, value);
            }
        }
        /// <summary>
        /// 获取key为value的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetItemScoreInSortedSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetItemScoreInSortedSet(key, value);
            }
        }
        /// <summary>
        /// 获取key所有集合的数据总数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetSortedSetCount(key);
            }
        }
        /// <summary>
        /// key集合数据从分数为fromscore到分数为toscore的数据总数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromScore"></param>
        /// <param name="toScore"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string key, double fromScore, double toScore)
        {
            //using (IRedisClient redis = GetClient())
            //{
            //    return redis.GetSortedSetCount(key, fromScore, toScore);
            //}\
            return 0;
        }
        /// <summary>
        /// 获取key集合从高分到低分排序数据，分数从fromscore到分数为toscore的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromscore"></param>
        /// <param name="toscore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSetByHighestScore(string key, double fromscore, double toscore)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeFromSortedSetByHighestScore(key, fromscore, toscore);
            }
        }
        /// <summary>
        /// 获取key集合从低分到高分排序数据，分数从fromscore到分数为toscore的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromscore"></param>
        /// <param name="toscore"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSetByLowestScore(string key, double fromscore, double toscore)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeFromSortedSetByLowestScore(key, fromscore, toscore);
            }
        }
        /// <summary>
        /// 获取key集合从高分到低分排序数据，分数从fromscore到分数为toscore的数据，带分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromscore"></param>
        /// <param name="toscore"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByHighestScore(string key, double fromscore, double toscore)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeWithScoresFromSortedSetByHighestScore(key, fromscore, toscore);
            }
        }
        /// <summary>
        /// 获取key集合从低分到高分排序数据，分数从fromscore到分数为toscore的数据，带分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromscore"></param>
        /// <param name="toscore"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetByLowestScore(string key, double fromscore, double toscore)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeWithScoresFromSortedSetByLowestScore(key, fromscore, toscore);
            }
        }
        /// <summary>
        /// 获取key集合数据，下标从fromRank到分数为toRank的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromRank"></param>
        /// <param name="toRank"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSet(string key, int fromRank, int toRank)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeFromSortedSet(key, fromRank, toRank);
            }
        }
        /// <summary>
        /// 获取key集合倒叙排列数据，下标从fromRank到分数为toRank的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromRank"></param>
        /// <param name="toRank"></param>
        /// <returns></returns>
        public static List<string> GetRangeFromSortedSetDesc(string key, int fromRank, int toRank)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeFromSortedSetDesc(key, fromRank, toRank);
            }
        }
        /// <summary>
        /// 获取key集合数据，下标从fromRank到分数为toRank的数据，带分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromRank"></param>
        /// <param name="toRank"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSet(string key, int fromRank, int toRank)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeWithScoresFromSortedSet(key, fromRank, toRank);
            }
        }
        /// <summary>
        /// 获取key集合倒叙排列数据，下标从fromRank到分数为toRank的数据，带分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromRank"></param>
        /// <param name="toRank"></param>
        /// <returns></returns>
        public static IDictionary<string, double> GetRangeWithScoresFromSortedSetDesc(string key, int fromRank, int toRank)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetRangeWithScoresFromSortedSetDesc(key, fromRank, toRank);
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除key为value的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool RemoveItemFromSortedSet(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }
        /// <summary>
        /// 删除下标从minRank到maxRank的key集合数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minRank"></param>
        /// <param name="maxRank"></param>
        /// <returns></returns>
        public static long RemoveRangeFromSortedSet(string key, int minRank, int maxRank)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveRangeFromSortedSet(key, minRank, maxRank);
            }
        }
        /// <summary>
        /// 删除分数从fromscore到toscore的key集合数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fromscore"></param>
        /// <param name="toscore"></param>
        /// <returns></returns>
        public static long RemoveRangeFromSortedSetByScore(string key, double fromscore, double toscore)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.RemoveRangeFromSortedSetByScore(key, fromscore, toscore);
            }
        }
        /// <summary>
        /// 删除key集合中分数最大的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PopItemWithHighestScoreFromSortedSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PopItemWithHighestScoreFromSortedSet(key);
            }
        }
        /// <summary>
        /// 删除key集合中分数最小的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PopItemWithLowestScoreFromSortedSet(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PopItemWithLowestScoreFromSortedSet(key);
            }
        }

        #endregion

        #region 其它
        /// <summary>
        /// 判断key集合中是否存在value数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SortedSetContainsItem(string key, string value)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.SortedSetContainsItem(key, value);
            }
        }
        /// <summary>
        /// 为key集合值为value的数据，分数加scoreby，返回相加后的分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="scoreBy"></param>
        /// <returns></returns>
        public static double IncrementItemInSortedSet(string key, string value, double scoreBy)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.IncrementItemInSortedSet(key, value, scoreBy);
            }
        }
        /// <summary>
        /// 获取keys多个集合的交集，并把交集添加的newkey集合中，返回交集数据的总数
        /// </summary>
        /// <param name="newkey"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static long StoreIntersectFromSortedSets(string newkey, string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.StoreIntersectFromSortedSets(newkey, keys);
            }
        }
        /// <summary>
        /// 获取keys多个集合的并集，并把并集数据添加到newkey集合中，返回并集数据的总数
        /// </summary>
        /// <param name="newkey"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static long StoreUnionFromSortedSets(string newkey, string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.StoreUnionFromSortedSets(newkey, keys);
            }
        }
        #endregion

        #region 保存数据DB文件到硬盘
        /// <summary>
        /// 保存数据DB文件到硬盘
        /// </summary>
        public static void Save()
        {
            using (IRedisClient redis = GetClient())
            {
                redis.Save();
            }
        }
        /// <summary>
        /// 异步保存数据DB文件到硬盘
        /// </summary>
        public static void SaveAsync()
        {
            using (IRedisClient redis = GetClient())
            {
                redis.SaveAsync();
            }
        }
        #endregion

        #region 清空DB
        /// <summary>
        /// 清空所有Db的Redis数据
        /// </summary>
        public static void FlushAll()
        {
            using (IRedisClient redis = GetClient())
            {
                redis.FlushAll();
            }
        }

        /// <summary>
        /// 清空当前Db的Redis数据
        /// </summary>
        public static void FlushDb()
        {
            using (IRedisClient redis = GetClient())
            {
                redis.FlushDb();
            }
        }
        #endregion

        #region Redis是否异常
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsNormal()
        {
            try
            {
                using (var redis = GetClient())
                {
                    return !redis.HadExceptions;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region --以下常用--
        #endregion

        #region 常用
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        public static void Set<T>(Dictionary<string, T> keyValues)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.SetAll<T>(keyValues);
            }
        }

        /// <summary>
        /// 添加键值对，指定时间过期
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, int minutes = 0)
        {
            if (minutes == 0) minutes = m_CacheTime;
            TimeSpan ts = new TimeSpan(0, minutes, 0);
            bool result = false;
            using (IRedisClient redis = GetClient())
            {
                result = redis.Set<T>(key, value, ts);
            }
            return result;
        }

        /// <summary>
        /// 返回指定Key的值 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            string result = null;
            using (IRedisClient redis = GetClient())
            {
                result = redis.GetValue(key);
            }
            if (!string.IsNullOrEmpty(result)) result = result.Trim('\"');
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            T result;
            using (IRedisClient redis = GetClient())
            {
                result = redis.Get<T>(key);
            }
            //if (!string.IsNullOrEmpty(result)) result = result.Trim('\"');
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        public static void Remove(List<string> keys)
        {
            using (IRedisClient redis = GetClient())
            {
                redis.RemoveAll(keys);
            }
        }
        #endregion

        #region 检索
        /// <summary>
        /// 根据表达式检索Key
        /// </summary>
        /// <param name="pattern">要匹配的表达式</param>
        /// <returns></returns>
        public static List<string> SearchKeys(string pattern)
        {
            using (var redis = GetClient())
            {
                return redis.SearchKeys(pattern);
            }
        }
        #endregion

        #region 设置过期时间
        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="minutes">过期分钟</param>
        /// <returns></returns>
        public static bool SetExpire(string key, int minutes)
        {
            if (minutes == 0) minutes = m_CacheTime;
            TimeSpan ts = new TimeSpan(0, minutes, 0);
            bool result = false;
            using (IRedisClient redis = GetClient())
            {
                result = redis.ExpireEntryIn(key, ts);
            }
            return result;
        }
        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        public static bool SetExpire(string key, DateTime expireTime)
        {
            bool result = false;
            using (IRedisClient redis = GetClient())
            {
                result = redis.ExpireEntryAt(key, expireTime);
            }
            return result;
        }
        #endregion

        #region SET处理

        /// <summary>
        /// 获取指定Set的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static HashSet<string> GetIntersectFromSets(List<string> keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetIntersectFromSets(keys.ToArray());
            }
        }

        /// <summary>
        /// 获取指定Set的交集
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static HashSet<string> GetIntersectFromSets(params string[] keys)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.GetIntersectFromSets(keys);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int PublishMessage(string key, string message)
        {
            using (IRedisClient redis = GetClient())
            {
                return redis.PublishMessage("redis:events:" + key, message);//发送消息
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void OnSubscribe(string key)
        {
            using (IRedisClient redis = GetClient())
            {
                using (var subscription = redis.CreateSubscription())
                {
                    subscription.OnSubscribe = channel =>
                    {
                        //订阅事件
                        QuickLib.Log.LogHelp.Logger("OnSubscribe");
                    };
                    subscription.OnUnSubscribe = channel =>
                    {
                        //退订事件
                        QuickLib.Log.LogHelp.Logger("退订事件");
                    };
                    subscription.OnMessage = (channel, msg) =>
                    {
                        QuickLib.Log.LogHelp.Logger("从频道：" + channel + "上接受到消息：" + msg + "");
                    };

                    subscription.SubscribeToChannels("redis:events:" + key); //blocking
                }
            }
        }
    }

    /// <summary>
    /// Redis客户端对象管理器
    /// </summary>
    internal class RedisManager
    {
        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        public PooledRedisClientManager CreateManager(string writeRedisHost, string readRedisHost, int db = 0)
        {
            var writeRedisHosts = writeRedisHost.Split('@');
            var readRedisHosts = readRedisHost.Split('@');
            return new PooledRedisClientManager(writeRedisHosts, readRedisHosts,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = 100,
                                 MaxReadPoolSize = 100,
                                 AutoStart = true
                             }, db, 50, 5);
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient(string writeRedisHost, string readRedisHost, int redisDB)
        {
            using (var pooledRedisClientManager = new RedisManager().CreateManager(writeRedisHost, readRedisHost))
            {
                var redis = pooledRedisClientManager.GetClient();
                redis.Db = redisDB;
                return redis;
            }
        }
    }
    #region 
    /*
  <configSections>
    <!--redis组件-->
    <sectionGroup name="ApplicationCache">
      <section name="Redis" type="System.Configuration.NameValueSectionHandler" />
      <section name="IISCache" type="System.Configuration.NameValueSectionHandler" />
    </sectionGroup>
  </configSections>
  <!--应用程序缓存-->
  <ApplicationCache>
    <Redis>
      <!--即可作为缓存使用，也可以作为数据库来使用-->
      <!--Redis数据库所在IP【上线需修改】，多个IP时用'@'符号隔开-->
      <add key="RedisWriteHostAddress" value="192.168.00.00:6379" />
      <add key="RedisReadHostAddress" value="192.168.00.00:6379" />
      <!--redis可以拥有多个数据库，每个应用程序可以使用单独的数据库存储数据（最大值不超过17，暂时不确定）-->
      <add key="RedisDb" value="3" />
      <!--缓存时间单位分钟-->
      <add key="RedisCacheTime" value="30" />
      <!--redis缓存key字符串不支持的符号（暂时就知道这些，后续继续加）-->
      <add key="KeyIgnoreChar" value="' ','\n'" />
    </Redis>
    <IISCache>
      <!--缓存时间单位分钟-->
      <add key="IISCacheTime" value="30" />
    </IISCache>
  </ApplicationCache>
*/
    /// <summary>
    /// 配置
    /// </summary>
    internal static class ApplictionCacheConfig
    {
        public const string m_ApplictionIISCacheConfigSectionName = "ApplicationCache/IISCache";
        public const string m_QuickCacheConfigSectionName = "ApplicationCache/QuickCache";
        public const string m_ApplictionRedisCacheConfigSectionName = "ApplicationCache/Redis";
        public static string RedisWriteHostAddress;
        public static string RedisReadHostAddress;
        public static string RedisDb;
        public static string RedisCacheTime;
        public static string KeyIgnoreChar;
        public static int IISCacheTime;
        public static int QuickCacheTime;
        static ApplictionCacheConfig()
        {
            NameValueCollection IISCacheSettings = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(m_ApplictionIISCacheConfigSectionName);
            NameValueCollection RedisCacheSettings = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(m_ApplictionRedisCacheConfigSectionName);
            NameValueCollection QuickCacheSettings = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(m_QuickCacheConfigSectionName);


            if (RedisCacheSettings != null)
            {
                RedisWriteHostAddress = RedisCacheSettings["RedisWriteHostAddress"];

                RedisReadHostAddress = RedisCacheSettings["RedisReadHostAddress"];

                RedisDb = RedisCacheSettings["RedisDb"];

                RedisCacheTime = RedisCacheSettings["RedisCacheTime"];

                KeyIgnoreChar = RedisCacheSettings["KeyIgnoreChar"];
            }
            if (IISCacheSettings != null)
            {
                string IISCacheTimeStr = IISCacheSettings["IISCacheTime"];
                if (IISCacheTimeStr == null || !int.TryParse(IISCacheTimeStr, out IISCacheTime)) IISCacheTime = 30;
            }
            if (QuickCacheSettings != null)
            {
                string QuickCacheTimeStr = QuickCacheSettings["QuickCacheTime"];
                if (QuickCacheTimeStr == null || !int.TryParse(QuickCacheTimeStr, out QuickCacheTime)) QuickCacheTime = 30;
            }
        }
    }
#endregion


    
}
