using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Weborb.Management.ServiceBrowser;

namespace Weborb.Registry
{
    public class MonitoredClassRegistry
    {
        private Hashtable selectedNodes = new Hashtable();

        public void addSelectedNode(ServiceNode node)
        {
            ServiceNode tempNode = null;

            while (true)
            {
                ServiceNode selectedNode = tempNode == null
                  ? (ServiceNode)selectedNodes[node.Name]
                  : (ServiceNode)tempNode.GetItem(node.Name);

                if (selectedNode != null)
                {
                    tempNode = selectedNode;
                    selectedNode.Selected = node.Selected;

                    if (selectedNode.Selected == ServiceNode.FULLY_SELECTED)
                    {
                        selectedNode.Items = new ServiceNode[0];

                        break;
                    }

                    node = (ServiceNode)node.Items[0];
                }
                else
                {
                    if (tempNode != null)
                        tempNode.AddItem(node);
                    else
                        selectedNodes[node.Name] = node;

                    break;
                }
            }
        }

        public void removeSelectedNode(ServiceNode node, String fullName)
        {
            String[] nameParts = fullName.Split(new char[] { '.' });
            int i = 0;
            ServiceNode tempNode = null;

            while (true)
            {
                String name = nameParts[i];
                i++;
                ServiceNode selectedNode = tempNode == null
                  ? (ServiceNode)selectedNodes[name]
                  : (ServiceNode)tempNode.GetItem(name);

                if (selectedNode.Selected != node.Selected)
                {
                    if (selectedNode.Parent == null)
                        if (node.Selected == ServiceNode.NOT_SELECTED)
                            selectedNodes.Remove(name);
                        else
                        {
                            selectedNodes[name] = node;

                            while (i < nameParts.Length)
                            {
                                node = node.GetItem(nameParts[i]);
                                i++;
                            }

                            node.Parent.RemoveItem(node);
                        }
                    else if (node.Selected == ServiceNode.NOT_SELECTED)
                        selectedNode.Parent.RemoveItem(selectedNode.Parent.GetItem(name));
                    else
                    {
                        selectedNode.Parent.RemoveItem(selectedNode.Parent.GetItem(name));
                        selectedNode.Parent.AddItem(node);

                        while (i < nameParts.Length)
                        {
                            node = node.GetItem(nameParts[i]);
                            i++;
                        }

                        node.Parent.RemoveItem(node);
                    }

                    break;
                }
                else
                {
                    tempNode = selectedNode;
                    node = (ServiceNode)node.GetItem(nameParts[i]);
                }
            }
        }

        public int isSelected(String parentName)
        {
            int selected = ServiceNode.NOT_SELECTED;
            String arguments = "";

            if (parentName.IndexOf("(") != -1)
            {
                arguments = parentName.Substring(parentName.IndexOf("("));
                parentName = parentName.Substring(0, parentName.IndexOf("("));
            }

            String[] nameParts = parentName.Split(new char[] { '.' });
            nameParts[nameParts.Length - 1] += arguments;
            ServiceNode node = null;

            for (int i = 0; i < nameParts.Length; i++)
            {
                if (i == 0)
                    node = (ServiceNode)selectedNodes[nameParts[i]];
                else
                    node = node.GetItem(nameParts[i]);

                if (node == null)
                    return 0;

                if (node.Selected == ServiceNode.FULLY_SELECTED)
                    return ServiceNode.FULLY_SELECTED;
            }

            selected = node != null ? node.Selected : ServiceNode.NOT_SELECTED;

            return selected;
        }

        public ArrayList getSelectedNodes()
        {
            ArrayList list = new ArrayList();
            IDictionaryEnumerator e = selectedNodes.GetEnumerator();

            while (e.MoveNext())
                list.Add(e.Value);

            return list;
        }

        public void clear()
        {
            selectedNodes = new Hashtable();
        }
    }
}
