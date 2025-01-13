<?php
$servername = "localhost";
$username = "maksymp";
$password = "35720465q";

// Create connection
$conn = new mysqli($servername, $username, $password);
mysqli_select_db($conn, 'maksymp');

// Prepare an array for the response
$response = array("status" => "success", "message" => "");

// Check connection
if ($conn->connect_error) {
    $response["status"] = "error";
    $response["message"] = "Connection failed: " . $conn->connect_error;
    echo json_encode($response);
    exit(); // Return nothing on error
}

$sql = "SELECT X, Y, Z, Id FROM DeathPositions ORDER BY Id DESC";
$result = $conn->query($sql);

$response = array();
if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $response[] = $row; // Add each row to the response array
    }
}
// Encode the response as JSON
echo json_encode($response);

// Close connection
$conn->close();
?>
