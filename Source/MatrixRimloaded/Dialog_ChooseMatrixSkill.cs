using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MatrixRimloaded;

public class Dialog_ChooseMatrixSkill : Window
{
    private readonly Ability_SkillHack skillHack;
    private Vector2 scrollPosition;
    private bool skillWasChosen;

    public Dialog_ChooseMatrixSkill(Ability_SkillHack skillHack)
    {
        this.skillHack = skillHack;
        doCloseButton = false;
        forcePause = true;
        doCloseX = false;
        closeOnClickedOutside = true;
        absorbInputAroundWindow = true;
    }

    public override Vector2 InitialSize => new Vector2(200f, 500f);

    public override void DoWindowContents(Rect inRect)
    {
        Text.Font = GameFont.Small;
        var outRect = new Rect(inRect);
        outRect.yMin += 20f;
        outRect.yMax -= 40f;
        outRect.width -= 16f;
        var viewRect = new Rect(0.0f, 0.0f, outRect.width + 128f,
            (float)((DefDatabase<SkillDef>.AllDefs.Count() * 35.0) + 100.0));
        Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, false);
        try
        {
            var y = 0.0f;

            var foundSkill = false;
            foreach (var skill in DefDatabase<SkillDef>.AllDefs)
            {
                foundSkill = true;
                var rect = new Rect(0.0f, y, viewRect.width * 0.7f, 32f)
                {
                    x = outRect.center.x - 63f,
                    width = viewRect.width * 0.5f
                };
                if (Widgets.ButtonText(rect, skill.defName))
                {
                    skillWasChosen = true;
                    skillHack.skillToLearn = skill;
                    SoundDefOf.Click.PlayOneShotOnCamera();
                    Close();
                    return;
                }

                y += 35f;
            }

            if (foundSkill)
            {
                y += 15f;
            }
        }
        finally
        {
            Widgets.EndScrollView();
        }
    }

    public override void PostClose()
    {
        if (!skillWasChosen)
        {
            //End targeting. But how?
        }

        base.PostClose();
    }
}