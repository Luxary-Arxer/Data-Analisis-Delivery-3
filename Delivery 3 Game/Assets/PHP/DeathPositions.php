<?php
$servername = "localhost";
$username = "maksymp";
$password = "35720465q";
$dbname = "maksymp";

// Enable error reporting for debugging
error_reporting(E_ALL);
ini_set('display_errors', 1);

// Create connection and check
$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    echo json_encode(["status" => "error", "message" => "Connection failed: " . $conn->connect_error]);
    exit();
}

// Check if the coordinates (X, Y, Z) are provided
if (isset($_POST['X']) && isset($_POST['Y']) && isset($_POST['Z'])) {
    // Get X, Y, Z values from POST
    $x = (float)$_POST['X']; // Ensure these are floats
    $y = (float)$_POST['Y'];
    $z = (float)$_POST['Z'];

    // Debug: Print received values
    echo json_encode([
        "status" => "info",
        "message" => "Received coordinates - X: $x, Y: $y, Z: $z"
    ]);

    // Prepare and execute SQL statement
    $stmt = $conn->prepare("INSERT INTO DeathPositions (X, Y, Z) VALUES (?, ?, ?)");
    
    // Check for prepare() failure
    if ($stmt === false) {
        echo json_encode(["status" => "error", "message" => "Prepare failed: " . $conn->error]);
        exit();
    }

    // Bind parameters (ensure the parameters are floats, use 'd' for double type)
    $stmt->bind_param("ddd", $x, $y, $z); // 'd' is for double (float)

    // Execute the statement
    if ($stmt->execute()) {
        echo json_encode(["status" => "success", "id" => $conn->insert_id]);
    } else {
        echo json_encode(["status" => "error", "message" => "Error inserting data"]);
    }

    $stmt->close();
} else {
    echo json_encode(["status" => "error", "message" => "Coordinates (X, Y, Z) not provided"]);
}

// Close connection
$conn->close();
?>
