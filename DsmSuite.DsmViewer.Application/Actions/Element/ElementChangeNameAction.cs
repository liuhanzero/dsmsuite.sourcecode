﻿using DsmSuite.DsmViewer.Application.Actions.Base;
using DsmSuite.DsmViewer.Application.Interfaces;
using DsmSuite.DsmViewer.Model.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace DsmSuite.DsmViewer.Application.Actions.Element
{
    public class ElementChangeNameAction : IAction
    {
        private readonly IDsmModel _model;
        private readonly IDsmElement _element;
        private readonly string _old;
        private readonly string _new;

        public const string TypeName = "echangename";

        public ElementChangeNameAction(object[] args)
        {
            Debug.Assert(args.Length == 2);
            _model = args[0] as IDsmModel;
            Debug.Assert(_model != null);
            IReadOnlyDictionary<string, string> data = args[1] as IReadOnlyDictionary<string, string>;
            Debug.Assert(data != null);

            ActionReadOnlyAttributes attributes = new ActionReadOnlyAttributes(_model, data);
            _element = attributes.GetElement(nameof(_element));
            Debug.Assert(_element != null);

            _old = attributes.GetString(nameof(_old));
            _new = attributes.GetString(nameof(_new));
        }

        public ElementChangeNameAction(IDsmModel model, IDsmElement element, string name)
        {
            _model = model;
            Debug.Assert(_model != null);

            _element = element;
            Debug.Assert(_element != null);

            _old = _element.Name;
            _new = name;
        }

        public string Type => TypeName;
        public string Title => "Change element name";
        public string Description => $"element={_element.Fullname} name={_old}->{_new}";

        public object Do()
        {
            _model.ChangeElementName(_element, _new);
            return null;
        }

        public void Undo()
        {
            _model.ChangeElementName(_element, _old);
        }

        public IReadOnlyDictionary<string, string> Data
        {
            get
            {
                ActionAttributes attributes = new ActionAttributes();
                attributes.SetInt(nameof(_element), _element.Id);
                attributes.SetString(nameof(_old), _old);
                attributes.SetString(nameof(_new), _new);
                return attributes.Data;
            }
        }
    }
}
