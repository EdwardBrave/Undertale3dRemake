using Entitas;
using UI;
using UnityEngine;
using Utils;

namespace Systems.UI
{
    public class BindingsSystem: IExecuteSystem
    {
        private readonly IGroup<UiEntity> _bondedWindows;


        public BindingsSystem(Contexts contexts)
        {
            _bondedWindows = contexts.ui.GetGroup(UiMatcher.Bindings);
        }
        
        public void Execute()
        {
            foreach (var entity in _bondedWindows.GetEntities())
            {
                if (!entity.bindings.context)
                {
                    if (!entity.hasView)
                        continue;
                    var binder = entity.view.obj.GetComponent<UIBinder>();
                    if (!binder)
                    {
                        Debug.LogWarning("Entity view object has no UIBinder for binding values");
                        entity.RemoveBindings();
                        continue;
                    }
                    entity.bindings.context = binder;
                }
                
                var context = entity.bindings.context;
                if (entity.bindings.sprites.IsChanged())
                {
                    foreach(var pair in entity.bindings.sprites.Changes())
                    {
                        var image = context.GetImage(pair.Key);
                        if (image != null)
                            image.Content.sprite = pair.Value;
                    }
                }
                if (entity.bindings.texts.IsChanged())
                {
                    foreach(var pair in entity.bindings.texts.Changes())
                    {
                        var text = context.GetText(pair.Key);
                        if (text != null)
                            text.Content.text = pair.Value;
                    }
                }
                if (entity.bindings.fields.IsChanged())
                {
                    foreach(var pair in entity.bindings.fields.Changes())
                    {
                        var field = context.GetField(pair.Key);
                        if (field != null)
                            field.Content.text = pair.Value;
                    }
                }

                foreach (var bonded in context.bondedFields)
                {
                    if (!bonded.Content)
                        continue;
                    if (!entity.bindings.fields.ContainsKey(bonded.name))
                        entity.bindings.fields[bonded.name] = new Changeable<string>();
                    var changeable = entity.bindings.fields[bonded.name];
                    changeable.SetQuietly(bonded.Content.text);
                }
            }
        }
    }
}