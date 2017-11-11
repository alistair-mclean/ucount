package me.alistairmclean.cc01;

import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;

public class RCControllerActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_rccontroller);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE);
        // Get the intent that started this activity and extract the string from the main activity
        Intent intent = getIntent();
    }
    //TODO - ADD RC CONTROLLER FUNCTIONALITY

}
