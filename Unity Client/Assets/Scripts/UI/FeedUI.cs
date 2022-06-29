using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedUI: MonoBehaviour
{
    [SerializeField] private GameObject feedPrefab;

    public void CreateFeed(string _text)
    {
        GameObject feed = Instantiate(feedPrefab, this.transform);
        feed.GetComponent<Text>().text = _text;
        feed.SetActive(true);
        StartCoroutine(DestroyAfter(feed, 5));
    }

    private IEnumerator DestroyAfter(GameObject _feed, int _timer)
    {
        yield return new WaitForSeconds(_timer);
        _feed.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(0.5f);
        Destroy(_feed);
    }
}
