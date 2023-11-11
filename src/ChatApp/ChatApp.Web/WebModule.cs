﻿using Autofac;
using ChatApp.Web.Models.Auth;

public class WebModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<RegisterModel>().AsSelf();
    }
}