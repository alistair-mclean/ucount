package me.alistairmclean.cc01;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void startCarActivity(View view) {
        Intent intent = new Intent(this, RCCarActivity.class);
        startActivity(intent);
    }

    public void startControllerActivity(View view) {
        Intent intent = new Intent(this, RCControllerActivity.class);
        startActivity(intent);
    }

}
