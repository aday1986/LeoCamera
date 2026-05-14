from ultralytics import YOLO

# 1. 加载模型
model = YOLO("yolov26n.pt")  # 你的 .pt 路径

# 2. 导出 ONNX
model.export(
    format="onnx"
)

print("✅ 导出完成，生成 best.onnx")
