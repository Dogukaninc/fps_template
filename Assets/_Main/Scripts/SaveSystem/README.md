# Unity Save System — Kurulum & Kullanım

## 📦 Gereksinimler

| Paket | Versiyon | Nasıl eklenir? |
|-------|----------|----------------|
| Newtonsoft.Json for Unity | ≥ 13.x | Package Manager → `com.unity.nuget.newtonsoft-json` |

---

## 📁 Dosya Yapısı

```
SaveSystem/
├── Runtime/
│   ├── SaveData.cs          ← Tüm save data'larının base class'ı
│   ├── ISaveable.cs         ← Save sistemine kaydolmak için arayüz
│   ├── JsonHelper.cs        ← Unity tipleri destekli JSON yardımcısı
│   ├── SaveSlotInfo.cs      ← Slot metadata (UI listesi için)
│   ├── SaveManager.cs       ← Ana yönetici (Singleton MonoBehaviour)
│   └── Example/
│       ├── GameSaveData.cs  ← Örnek oyuna özgü save data
│       └── PlayerController.cs  ← ISaveable örnek implementasyonu
└── Editor/
    ├── SaveSystemEditorWindow.cs  ← Tools menüsü Editor Tool
    └── SaveManagerEditor.cs       ← Custom Inspector
```

---

## 🚀 Hızlı Başlangıç

### 1. SaveManager'ı Sahneye Ekle
- Hiyerarşide boş bir GameObject oluştur → `SaveManager` adını ver.
- `SaveManager.cs` scriptini ekle.
- Inspector'da `Max Slots`, `Encrypt Saves` ayarlarını yap.

### 2. Kendi SaveData'nı Oluştur
```csharp
[Serializable]
public class MyGameSave : SaveData   // SaveData'dan türet
{
    public string playerName;
    public int    level;
    public Vector3 position;         // Unity tipleri desteklenir
    public Color  hairColor;
    public Dictionary<string, int> questProgress = new();
    // ... istediğin her tipi ekle
}
```

### 3. ISaveable'ı Implement Et
```csharp
public class PlayerController : MonoBehaviour, ISaveable
{
    public string SaveId => "PlayerController";   // benzersiz ID

    public SaveData CaptureState()
    {
        return new MyGameSave { playerName = name, level = lvl, position = transform.position };
    }

    public void RestoreState(SaveData data)
    {
        if (data is not MyGameSave d) return;
        name               = d.playerName;
        lvl                = d.level;
        transform.position = d.position;
    }
}
```

### 4. Kaydet / Yükle
```csharp
// Kaydet
SaveManager.Instance.Save(slotIndex: 0, data: myGameSave);

// Yükle
var data = SaveManager.Instance.Load<MyGameSave>(slotIndex: 0);

// Slot listesi (UI için)
var infos = SaveManager.Instance.GetAllSlotInfos();
foreach (var info in infos)
    Debug.Log($"Slot {info.slotIndex}: {info.saveSlotName} - {info.FormattedPlayTime}");

// Sil
SaveManager.Instance.DeleteSlot(slotIndex: 0);
```

---

## 🛠 Editor Tool

**Unity Menu → Tools → Save System Editor**

| Özellik | Açıklama |
|---------|----------|
| Slot Listesi | Tüm dolu/boş slotları ve metadata bilgilerini gösterir |
| Editörde Yükle | Play'e basmadan sahnedeki ISaveable nesnelere veriyi restore eder |
| Ham JSON Görünümü | Save dosyasının JSON içeriğini okuyabilir / düzenleyebilirsin |
| JSON Kaydet | Düzenlenen JSON'u doğrudan dosyaya yazar |
| Slot Sil | İstenen slotu siler |
| Klasör Aç | Save dizinini Finder/Explorer'da açar |

---

## 🔐 Şifreleme

`Encrypt Saves = true` yapılırsa dosyalar XOR + Base64 ile şifrelenir.  
Prodüksiyon için AES kullanmak istersen `JsonHelper.cs`'deki `Encrypt/Decrypt` metodlarını genişlet.

---

## 💡 İpuçları

- **TypeNameHandling.Auto** sayesinde polimorfik tipler (base class referansları) JSON'da doğru serialize edilir.
- `Vector2/3/4`, `Quaternion`, `Color` Unity tipleri kutudan çıkar desteklenir.
- `Dictionary<string, T>` ve `List<T>` doğrudan kullanabilirsin.
- Birden fazla ISaveable varsa `CaptureAllSaveables()` / `RestoreAllSaveables()` ile toplu işlem yapabilirsin.
