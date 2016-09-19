using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class PatrolBrain : IEnemyBrain {

	public List <Structs.EnemyAction> e_Actions = new List <Structs.EnemyAction> () {
		new Structs.EnemyAction (EnemyActionType.MOVE, Direction.FORWARD, 1)
	};

	public override InputFeeder Think (EnemyController ectrl, InputFeeder ifeed) {
		if (ifeed.IsNull) {
			string feed = "";
			e_Actions.ForEach ((action) => {
				if (action.IsGood) {
					feed += action.AsString;
				}
			});
			return new InputFeeder (feed);
		} else {
			return ifeed;
		}
	}
}