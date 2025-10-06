using System.Collections.Generic;
using UnityEngine;

public class CharacterGunHandler : MonoBehaviour {
    [SerializeField] private CharacterStatsData statsData;
    [SerializeField] private CharacterStatsManager statsManager;
    [SerializeField] private Transform rightPos;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform upPos;
    [SerializeField] private Transform downPos;
    [SerializeField] private List<Transform> guns = new();

    private List<Transform> unlocked = new();
    private int activeIndex;

    public IGunBehaviour Gun => unlocked[activeIndex].GetComponent<IGunBehaviour>();
    public SpriteRenderer GunSpriteRenderer => unlocked[activeIndex].GetComponent<IGunBehaviour>().Renderer;
    public List<Sprite> GunSprites => unlocked[activeIndex].GetComponent<IGunBehaviour>().Sprites;
    public float GunWeight => unlocked[activeIndex].GetComponent<IGunBehaviour>().Weight;


    public void Previous() => Equip((activeIndex + 1) % unlocked.Count);
    public void Next() => Equip((activeIndex + 1) % unlocked.Count);

    public void AdjustPos(Vector2 faceDir) {
        if (faceDir == Vector2.up) {
            if (transform.position != upPos.position) transform.position = upPos.position;
            if (GunSpriteRenderer.sprite != GunSprites[1]) GunSpriteRenderer.sprite = GunSprites[1];
            if (GunSpriteRenderer.flipY) GunSpriteRenderer.flipY = false;
        }
        else if (faceDir == Vector2.right) {
            if (transform.position != rightPos.position) transform.position = rightPos.position;
            if (GunSpriteRenderer.sprite != GunSprites[0]) GunSpriteRenderer.sprite = GunSprites[0];
            if (GunSpriteRenderer.flipY) GunSpriteRenderer.flipY = false;
        }
        else if (faceDir == Vector2.left) {
            if (transform.position != leftPos.position) transform.position = leftPos.position;
            if (GunSpriteRenderer.sprite != GunSprites[0]) GunSpriteRenderer.sprite = GunSprites[0];
            if (!GunSpriteRenderer.flipY) GunSpriteRenderer.flipY = true;
        }
        else {
            if (transform.position != downPos.position) transform.position = downPos.position;
            if (GunSpriteRenderer.sprite != GunSprites[1]) GunSpriteRenderer.sprite = GunSprites[1];
            if (GunSpriteRenderer.flipY) GunSpriteRenderer.flipY = false;
        }
    }

    private void Start() {
        // unlocked.Add(guns[0]);
        foreach (var gun in guns) {
            gun.GetComponent<IGunBehaviour>().CharStatData = statsData;
            gun.GetComponent<IGunBehaviour>().CharStatManager = statsManager;
            gun.gameObject.SetActive(false);
            unlocked.Add(gun);
        }
        activeIndex = 0;
        unlocked[0].gameObject.SetActive(true);
    }

    private void Equip(int i) {
        if (i < 0 || i >= guns.Count || i == activeIndex) return;
        unlocked[activeIndex].gameObject.SetActive(false);
        unlocked[i].gameObject.SetActive(true);
        activeIndex = i;
    }
}
