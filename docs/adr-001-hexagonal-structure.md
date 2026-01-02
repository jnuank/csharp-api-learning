# ADR 1 — Controller/Usecase/Gateway/Driver 構成

- **ステータス**: 採用
- **コンテキスト**: API では Todo アイテムの集約操作・状態遷移と HTTP エンドポイントの責務を分けたかった。`Domain/TodoItem.cs` に値とイベントの定義、`Usecase` 以下にユースケース、`Usecase/Port` に依存性の抽象、`Gateway` にその実装、`Driver` に具体的な永続化処理がある。Controller から先は依存性逆転の構造にして、ユースケースが Port に依存することで実装を差し替えられるようにしたい。
- **決定**: `Controller` → `Usecase` → `Port` → `Gateway` → `Driver` の流れを守る。`TodoController` などのエンドポイントはユースケースを呼び、ユースケースは `ITodoItemPort` だけに依存し、実装は `TodoItemGateway` が担って PostgresDriver を使う。`Program.cs` に必要なサービスをすべて登録して DI でつなぐ。
- **結果**: Controller や Usecase のテスト／差し替え時に Port や Gateway をモックするだけで足りる。Postgres 固有コードは `Driver/TodoItems/PostgresDriver.cs` に閉じていて、ドメインとユースケースは純粋に保てる。

