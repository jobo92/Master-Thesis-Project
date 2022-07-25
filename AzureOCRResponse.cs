using System;

[Serializable]
public class AzureOCRResponse
{
    public string status;
    public string createdDateTime;
    public string lastUpdatedDateTime;
    public AzureOCRAnalyzeResult analyzeResult;
}

[Serializable]
public class AzureOCRAnalyzeResult
{
    public string version;
    public string modelVersion;
    public AzureOCRreadResult[] readResults;
}

[Serializable]
public class AzureOCRreadResult
{
    public int page;
    public int angle;
    public int width;
    public int height;
    public string unit;
    public AzureOCRLine[] lines;
}


[Serializable]
public class AzureOCRLine
{
    public int[] boundingBox;
    public string text;
    public AzureOCRWord[] words;
}

[Serializable]
public class AzureOCRWord
{
    public int[] boundingBox;
    public string text;
    public float confidence;

}

/* 
"status": "succeeded",
    "createdDateTime": "2022-04-13T18:07:15Z",
    "lastUpdatedDateTime": "2022-04-13T18:07:16Z",
    "analyzeResult": {
        "version": "3.2.0",
        "modelVersion": "2021-04-12",
        "readResults": [
            {
                "page": 1,
                "angle": 0,
                "width": 2220,
                "height": 1080,
                "unit": "pixel",
                "lines": [
                    {
                        "boundingBox": [
                            1240,
                            266,
                            1669,
                            264,
                            1669,
                            317,
                            1240,
                            319
                        ],
                        "text": "Testing if it works",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1240,
                                    267,
                                    1413,
                                    267,
                                    1414,
                                    319,
                                    1242,
                                    318
                                ],
                                "text": "Testing",
                                "confidence": 0.994
                            },
                            {
                                "boundingBox": [
                                    1424,
                                    267,
                                    1461,
                                    266,
                                    1461,
                                    319,
                                    1424,
                                    319
                                ],
                                "text": "if",
                                "confidence": 0.999
                            },
                            {
                                "boundingBox": [
                                    1471,
                                    266,
                                    1505,
                                    266,
                                    1504,
                                    319,
                                    1471,
                                    319
                                ],
                                "text": "it",
                                "confidence": 0.999
                            },
                            {
                                "boundingBox": [
                                    1515,
                                    266,
                                    1668,
                                    264,
                                    1667,
                                    317,
                                    1514,
                                    319
                                ],
                                "text": "works",
                                "confidence": 0.996
                            }
                        ]
                    },
                    {
                        "boundingBox": [
                            1237,
                            322,
                            1519,
                            322,
                            1519,
                            374,
                            1237,
                            374
                        ],
                        "text": "bigger than",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1240,
                                    323,
                                    1391,
                                    324,
                                    1392,
                                    375,
                                    1240,
                                    374
                                ],
                                "text": "bigger",
                                "confidence": 0.996
                            },
                            {
                                "boundingBox": [
                                    1401,
                                    324,
                                    1511,
                                    323,
                                    1513,
                                    375,
                                    1402,
                                    375
                                ],
                                "text": "than",
                                "confidence": 0.994
                            }
                        ]
                    },
                    {
                        "boundingBox": [
                            1239,
                            380,
                            1332,
                            380,
                            1331,
                            424,
                            1240,
                            424
                        ],
                        "text": "9>8",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1241,
                                    380,
                                    1320,
                                    380,
                                    1320,
                                    424,
                                    1241,
                                    424
                                ],
                                "text": "9>8",
                                "confidence": 0.994
                            }
                        ]
                    },
                    {
                        "boundingBox": [
                            1239,
                            439,
                            1535,
                            437,
                            1535,
                            482,
                            1239,
                            484
                        ],
                        "text": "smaller than",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1242,
                                    441,
                                    1417,
                                    438,
                                    1418,
                                    483,
                                    1244,
                                    484
                                ],
                                "text": "smaller",
                                "confidence": 0.994
                            },
                            {
                                "boundingBox": [
                                    1426,
                                    438,
                                    1529,
                                    437,
                                    1529,
                                    483,
                                    1427,
                                    483
                                ],
                                "text": "than",
                                "confidence": 0.994
                            }
                        ]
                    },
                    {
                        "boundingBox": [
                            1241,
                            493,
                            1357,
                            493,
                            1357,
                            537,
                            1240,
                            536
                        ],
                        "text": "7<10",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1242,
                                    493,
                                    1347,
                                    493,
                                    1347,
                                    537,
                                    1242,
                                    537
                                ],
                                "text": "7<10",
                                "confidence": 0.986
                            }
                        ]
                    },
                    {
                        "boundingBox": [
                            1238,
                            553,
                            1520,
                            549,
                            1521,
                            600,
                            1239,
                            606
                        ],
                        "text": "equal 5 == 5",
                        "appearance": {
                            "style": {
                                "name": "other",
                                "confidence": 0.878
                            }
                        },
                        "words": [
                            {
                                "boundingBox": [
                                    1239,
                                    555,
                                    1375,
                                    551,
                                    1375,
                                    603,
                                    1240,
                                    607
                                ],
                                "text": "equal",
                                "confidence": 0.932
                            },
                            {
                                "boundingBox": [
                                    1385,
                                    550,
                                    1413,
                                    550,
                                    1413,
                                    602,
                                    1386,
                                    603
                                ],
                                "text": "5",
                                "confidence": 0.996
                            },
                            {
                                "boundingBox": [
                                    1423,
                                    550,
                                    1478,
                                    549,
                                    1478,
                                    602,
                                    1423,
                                    602
                                ],
                                "text": "==",
                                "confidence": 0.996
                            },
                            {
                                "boundingBox": [
                                    1488,
                                    549,
                                    1518,
                                    549,
                                    1518,
                                    602,
                                    1488,
                                    602
                                ],
                                "text": "5",
                                "confidence": 0.996
                            }
                        ]
                    }
                ]
            }
        ]
    }
}
*/