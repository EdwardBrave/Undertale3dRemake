//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class DataMatcher {

    public static Entitas.IAllOfMatcher<DataEntity> AllOf(params int[] indices) {
        return Entitas.Matcher<DataEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<DataEntity> AllOf(params Entitas.IMatcher<DataEntity>[] matchers) {
          return Entitas.Matcher<DataEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<DataEntity> AnyOf(params int[] indices) {
          return Entitas.Matcher<DataEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<DataEntity> AnyOf(params Entitas.IMatcher<DataEntity>[] matchers) {
          return Entitas.Matcher<DataEntity>.AnyOf(matchers);
    }
}