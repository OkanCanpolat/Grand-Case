# Grand-Case
- Video kaydı Unity Recorder ile kaydelmiştir. Recorder 2436x1125 portrait modunda kayıt almaya izin vermediği için kayıt 2160x1080 portrait çözünürlüğü kullanılarak alınmıştır.

- Board oluşturulurken ve spin sırasında yeni tile'lar oluşturulerken optimizasyon sağlanması için Object Pool yöntemi kullanılmıştır.

- Drawcall sayısını azaltmak için Sprite Atlas kullanılarak Dynamic Batching kullanılmıştır.
- Case dokümanında belirtildiği gibi UI ögeleri için Unity'nin Canvas sistemi kullanılmamıştır. Bunun yerine UI ögeleri sprite'lar kullanılarak oluşturulmuştur.
- Dokümanda "Optional" olarak verilen görev  BFS (Breadth First Search) algoritması kullanılarak yapılmıştır.
- MVP (Model View Presenter), State, Event Bus, Facade gibi patternler kullanılmıştır.
- Dependecy Injection, Zenject kullanılarak sağlanmıştır.
- Asenkron işlemler için "Coroutine" ve "Task/await" sistemi kullanılmıştır.
- Build, Profiler kullanılarak çeşitli cihazlarda optimizasyon açısından test edilmiştir. Target FPS 60 olarak ayarlandığında bile sorunsuz şekilde çalışmaktadır.
- Bazı animasyonlar için iTween kütüphanesi kullanılmıştır.
