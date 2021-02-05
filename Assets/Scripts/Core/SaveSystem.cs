using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Data;
using Entitas;
using UnityEngine;

namespace Logic.Systems.Core
{
    public class SaveSystem: ReactiveSystem<CoreEntity>
    {

        private static string _filePath = "/UserData.data";
        private readonly CoreContext _coreContext;


        public SaveSystem(Contexts contexts) : base(contexts.core)
        {
            _coreContext = contexts.core;
            // TODO implement save system
            // _coreContext.SetUserData(LoadProgress() ?? new UserData());
        }
        
        protected override ICollector<CoreEntity> GetTrigger(IContext<CoreEntity> context)
        {
            return context.CreateCollector(/*CoreMatcher.UpdateUserProgress*/);
        }

        protected override bool Filter(CoreEntity entity)
        {
            return true;
        }

        protected override void Execute(List<CoreEntity> entities)
        {
            foreach (var entity in entities){}
                /*entity.isUpdateUserProgress = false;
            SaveProgress(_coreContext.userProgress.data);*/
        }

        public static void SaveProgress(UserData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + _filePath, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        
        public static UserData LoadProgress()
        {
            var path = Application.persistentDataPath + _filePath;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                var userData = (UserData)formatter.Deserialize(stream);
                stream.Close();
                return userData;
            }
            Debug.LogWarning("Save file is not found in " + path);
            return null;
        }
    }
}