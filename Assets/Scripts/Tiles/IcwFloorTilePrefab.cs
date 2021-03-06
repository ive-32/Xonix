using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class IcwFloorTilePrefab : IcwBaseTile
    {
        public GameObject DestroyedFloorTile;
        protected override void Start()
        {
            objtype = IcwGrid.FieldObjectsTypes.Field;
            base.Start();
            transform.SetParent(IcwObjects.FieldTiles.transform);
        }

        public override void DestroyTile()
        {
            base.DestroyTile();
            GameObject gb = Instantiate(DestroyedFloorTile, this.transform.position, Quaternion.identity);
            Animator anim = gb.GetComponent<Animator>();
            anim.Play("DestroyFieldTile");
            Destroy(gb, anim.GetCurrentAnimatorStateInfo(0).length);
        }

    }

}
