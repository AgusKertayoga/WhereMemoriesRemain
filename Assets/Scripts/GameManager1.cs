using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    [Header("Settings")]
    [Range(2, 6)]
    [SerializeField] private int size = 4;

    [SerializeField] private float gapThickness = 0.01f;

    private List<Transform> pieces = new List<Transform>();
    private int emptyLocation;
    private bool shuffling;

    private void Awake()
    {
        if (gameTransform == null)
        {
            GameObject board = GameObject.Find("GameBoard");

            if (board != null)
                gameTransform = board.transform;
        }
    }

    private void Start()
    {
        if (gameTransform == null)
        {
            Debug.LogError("GameBoard belum diisi!");
            return;
        }

        if (piecePrefab == null)
        {
            Debug.LogError("Piece Prefab belum diisi!");
            return;
        }

        CreateGamePieces();

        StartCoroutine(WaitShuffle(0.5f));
    }

    private void CreateGamePieces()
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);

                pieces.Add(piece);

                piece.localPosition = new Vector3(
                    -1 + (2 * width * col) + width,
                     1 - (2 * width * row) - width,
                     0);

                piece.localScale = Vector3.one * ((2 * width) - gapThickness);

                piece.name = (row * size + col).ToString();

                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = pieces.Count - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    MeshFilter mf = piece.GetComponent<MeshFilter>();

                    if (mf != null)
                    {
                        Mesh mesh = mf.mesh;

                        float gap = gapThickness / 2f;

                        Vector2[] uv = new Vector2[4];

                        uv[0] = new Vector2(width * col + gap,
                            1 - ((width * (row + 1)) - gap));

                        uv[1] = new Vector2(width * (col + 1) - gap,
                            1 - ((width * (row + 1)) - gap));

                        uv[2] = new Vector2(width * col + gap,
                            1 - ((width * row) + gap));

                        uv[3] = new Vector2(width * (col + 1) - gap,
                            1 - ((width * row) + gap));

                        mesh.uv = uv;
                    }
                }
            }
        }

        Debug.Log("Puzzle dibuat : " + pieces.Count + " pieces.");
    }

    private void Update()
    {
        if (!shuffling && CheckCompletion())
        {
            shuffling = true;
            StartCoroutine(WaitShuffle(1f));
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero);

            if (!hit)
                return;

            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == hit.transform)
                {
                    if (SwapIfValid(i, -size, size)) break;
                    if (SwapIfValid(i, size, size)) break;
                    if (SwapIfValid(i, -1, 0)) break;
                    if (SwapIfValid(i, 1, size - 1)) break;
                }
            }
        }
    }

    private bool SwapIfValid(int index, int offset, int colCheck)
    {
        if ((index % size) != colCheck &&
            index + offset == emptyLocation)
        {
            Transform temp = pieces[index];

            pieces[index] = pieces[index + offset];
            pieces[index + offset] = temp;

            Vector3 pos = pieces[index].localPosition;

            pieces[index].localPosition =
                pieces[index + offset].localPosition;

            pieces[index + offset].localPosition = pos;

            emptyLocation = index;

            return true;
        }

        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != i.ToString())
                return false;
        }

        return true;
    }

    private IEnumerator WaitShuffle(float time)
    {
        yield return new WaitForSeconds(time);

        Shuffle();

        shuffling = false;
    }

    private void Shuffle()
    {
        int moves = size * size * size;
        int last = -1;

        while (moves > 0)
        {
            int rnd = Random.Range(0, pieces.Count);

            if (rnd == last)
                continue;

            last = emptyLocation;

            if (SwapIfValid(rnd, -size, size) ||
                SwapIfValid(rnd, size, size) ||
                SwapIfValid(rnd, -1, 0) ||
                SwapIfValid(rnd, 1, size - 1))
            {
                moves--;
            }
        }
    }
}