using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIK : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RigBuilder rigBuilder;

    [Header("Constraints")]
    [SerializeField] private TwoBoneIKConstraint[] twoBoneIKConstraints;
    [SerializeField] private MultiAimConstraint[] multiAimConstraints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigureIK(Transform ikTarget)
    {
        //Enable the rig builder
        rigBuilder.enabled = true;

        foreach (var twoBoneIKConstraint in twoBoneIKConstraints)
        {
            twoBoneIKConstraint.data.target = ikTarget;
        }

        foreach(var multiAimConstraint in multiAimConstraints)
        {
            WeightedTransformArray weightedTransforms = new WeightedTransformArray();
            weightedTransforms.Add(new WeightedTransform(ikTarget, 1));

            multiAimConstraint.data.sourceObjects = weightedTransforms;
        }

        rigBuilder.Build();
    }

    public void DisableIK()
    {
        rigBuilder.enabled = false;
    }
}
