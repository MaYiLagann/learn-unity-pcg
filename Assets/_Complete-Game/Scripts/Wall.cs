using UnityEngine;

namespace Completed
{
    public class Wall : MonoBehaviour
    {
        // 1 of 2 audio clips that play when the wall is attacked by the player.
        public AudioClip chopSound1;
        // 2 of 2 audio clips that play when the wall is attacked by the player.
        public AudioClip chopSound2;
        // Alternate sprite to display after Wall has been attacked by player.
        public Sprite dmgSprite;
        // hit points for the wall.
        public int hp = 3;

        public GameObject[] foodTiles;

        // Store a component reference to the attached SpriteRenderer.
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            // Get a component reference to the SpriteRenderer.
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // DamageWall is called when the player attacks a wall.
        public void DamageWall(int loss)
        {
            // Call the RandomizeSfx function of SoundManager to play one of two chop sounds.
            SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

            // Set spriteRenderer to the damaged wall sprite.
            spriteRenderer.sprite = dmgSprite;

            // Subtract loss from hit point total.
            hp -= loss;

            // If hit points are less than or equal to zero:
            if (hp <= 0)
            {
                if (foodTiles.Length != 0 && Random.Range(0, 5) == 1)
                {
                    var toInstantiate = foodTiles[Random.Range(0, foodTiles.Length)];
                    var instance = Instantiate(toInstantiate, transform.position, Quaternion.identity);
                    instance.transform.SetParent(transform.parent);
                }

                // Disable the gameObject.
                gameObject.SetActive(false);
            }
        }
    }
}
