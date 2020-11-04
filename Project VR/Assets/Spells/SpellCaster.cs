using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("List of spells corresponding to the order of names entered in the MLanalyzer script.")]
    private GameObject[] m_Spells;

    void Start()
    {
        MLanalyzer.OnSymbolRecognized += ChooseSpell;
    }

    private void ChooseSpell(int spell)
    {
        if (m_Spells[spell] != null)
            Instantiate(m_Spells[spell], Vector3.up*1000, 
                Quaternion.identity);
    }
}
