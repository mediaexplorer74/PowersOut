
// Type: BlueJay.Component.System.ServiceCollectionExtensions
// Assembly: BlueJay.Component.System, Version=0.7.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA1EE518-FBA9-4E27-A6D9-52CA1E9C97D5
// Assembly location: BlueJay.Component.System.dll inside C:\Users\Admin\Desktop\RE\PowersOut\PowersOut.exe)

using BlueJay.Component.System.Collections;
using BlueJay.Component.System.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace BlueJay.Component.System
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBlueJaySystem(this IServiceCollection collection)
    {
      return collection.AddSingleton<IFontCollection, FontCollection>().AddScoped<ILayers, Layers>().AddTransient<IQuery, AllQuery>().AddTransient(typeof (IQuery<>), typeof (Query<>)).AddTransient(typeof (IQuery<,>), typeof (Query<,>)).AddTransient(typeof (IQuery<,,>), typeof (Query<,,>)).AddTransient(typeof (IQuery<,,,>), typeof (Query<,,,>)).AddTransient(typeof (IQuery<,,,,>), typeof (Query<,,,,>)).AddTransient(typeof (IQuery<,,,,,>), typeof (Query<,,,,,>)).AddTransient(typeof (IQuery<,,,,,,>), typeof (Query<,,,,,,>)).AddTransient(typeof (IQuery<,,,,,,,>), typeof (Query<,,,,,,,>)).AddTransient(typeof (IQuery<,,,,,,,,>), typeof (Query<,,,,,,,,>)).AddTransient(typeof (IQuery<,,,,,,,,,>), typeof (Query<,,,,,,,,,>));
    }
  }
}
